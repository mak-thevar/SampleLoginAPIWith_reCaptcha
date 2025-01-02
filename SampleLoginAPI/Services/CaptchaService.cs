namespace SampleLoginAPI.Services;

public class CaptchaService : ICaptchaService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CaptchaService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> VerifyCaptchaAsync(string captchaToken)
    {
        var secretKey = _configuration["CaptchaSettings:SecretKey"];
        var verificationUrl = _configuration["CaptchaSettings:VerificationUrl"];

        var response = await _httpClient.PostAsync($"{verificationUrl}?secret={secretKey}&response={captchaToken}", null);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var result = await response.Content.ReadFromJsonAsync<RecaptchaResponse>();
        return result != null && result.Success && result.Score >= 0.5; // Adjust score threshold for reCAPTCHA v3
    }
}
