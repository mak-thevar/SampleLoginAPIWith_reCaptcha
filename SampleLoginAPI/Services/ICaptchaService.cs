namespace SampleLoginAPI.Services;

public interface ICaptchaService
{
    Task<bool> VerifyCaptchaAsync(string captchaToken);
}

public class RecaptchaResponse
{
    public bool Success { get; set; }
    public float Score { get; set; } // Only for reCAPTCHA v3
    public string[]? ErrorCodes { get; set; }
}
