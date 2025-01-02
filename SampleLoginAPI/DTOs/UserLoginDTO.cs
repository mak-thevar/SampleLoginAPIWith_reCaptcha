namespace SampleLoginAPI.DTOs;
public class UserLoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CaptchaToken { get; set; } = string.Empty; // Optional if CAPTCHA is not triggered
}
