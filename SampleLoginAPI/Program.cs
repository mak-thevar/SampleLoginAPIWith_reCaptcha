using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using SampleLoginAPI.Data;
using SampleLoginAPI.DTOs;
using SampleLoginAPI.Services;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<ICaptchaService, CaptchaService>();


// Configure JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles(); // Serves files from wwwroot by default

// Migrate Database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Endpoints
app.UseAuthentication();
app.UseAuthorization();


app.MapPost("/register", async (UserRegisterDto dto, IUserService userService) =>
{
    var result = await userService.RegisterUserAsync(dto);
    return result.IsSuccess
        ? Results.Ok(result.Message)
        : Results.BadRequest(result.Message);
}).WithOpenApi();


app.MapPost("/v1/login", async (UserLoginDto dto, IUserService userService) =>
{
    var result = await userService.LoginUserAsync(dto);
    return result.IsSuccess
        ? Results.Ok(new { Token = result.Message })
        : Results.Problem(detail: "Invalid Credentials",statusCode: 401);
});


app.MapPost("/v2/login", async (UserLoginDto dto, IUserService userService, ICaptchaService captchaService, IMemoryCache memoryCache) =>
{
    var maxFailedAttempts = int.Parse(builder.Configuration["LoginSettings:MaxFailedAttempts"]);
    var lockoutDuration = TimeSpan.FromMinutes(int.Parse(builder.Configuration["LoginSettings:LockoutDurationInMinutes"]));
    var cacheKey = $"FailedAttempts:{dto.Username}";

    // Get current failed attempts
    memoryCache.TryGetValue(cacheKey, out int failedAttempts);

    // If max failed attempts exceeded, require CAPTCHA validation
    if (failedAttempts >= maxFailedAttempts)
    {
        var isCaptchaValid = await captchaService.VerifyCaptchaAsync(dto.CaptchaToken);
        if (!isCaptchaValid)
        {
            return Results.BadRequest("Invalid CAPTCHA.");
        }
    }

    // Authenticate User
    var result = await userService.LoginUserAsync(new UserLoginDto
    {
        Username = dto.Username,
        Password = dto.Password
    });

    if (!result.IsSuccess)
    {
        // Increment failed attempts and set lockout duration
        failedAttempts++;
        memoryCache.Set(cacheKey, failedAttempts, lockoutDuration);

        return Results.Unauthorized();
    }

    // Reset failed attempts on successful login
    memoryCache.Remove(cacheKey);

    return Results.Ok(new { Token = result.Message });

}).WithOpenApi();

app.MapGet("/secure", () => "This is a secure endpoint!")
    .RequireAuthorization().WithOpenApi();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();