# SampleLoginAPIWith_reCaptcha

This repository demonstrates how to integrate Google reCAPTCHA into a .NET 8 Web API application to enhance security by preventing automated bot logins and mitigating brute force attacks.

## Features

- **User Registration**: Allows new users to register with a username and password.
- **User Login**: Enables existing users to log in with their credentials using two versions of login APIs.
- **reCAPTCHA Integration**: Implements Google reCAPTCHA to verify human users during the login process, especially after multiple failed login attempts.
- **Secure Endpoints**: Includes an example of a protected endpoint requiring authentication.

## API Endpoints

### 1. User Registration
- **Endpoint**: `/register`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- **Response**:
  - `200 OK` if registration is successful.
  - `400 Bad Request` if the username already exists or validation fails.

### 2. User Login (Basic Authentication)
- **Endpoint**: `/v1/login`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- **Response**:
  - `200 OK` with a JWT token on successful login.
  - `401 Unauthorized` if credentials are invalid.

### 3. User Login with reCAPTCHA
- **Endpoint**: `/v2/login`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "username": "string",
    "password": "string",
    "captchaToken": "string" // Required after multiple failed attempts
  }
  ```
- **Response**:
  - `200 OK` with a JWT token on successful login.
  - `400 Bad Request` if reCAPTCHA validation fails.
  - `401 Unauthorized` if credentials are invalid.

### 4. Secure Endpoint
- **Endpoint**: `/secure`
- **Method**: `GET`
- **Response**:
  - `200 OK` if the user is authenticated.
  - `401 Unauthorized` if authentication is missing or invalid.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/download.html)
- [Google reCAPTCHA v3](https://www.google.com/recaptcha/admin/create) (Site Key and Secret Key)

## Getting Started

### Clone the Repository
```bash
git clone https://github.com/mak-thevar/SampleLoginAPIWith_reCaptcha.git
cd SampleLoginAPIWith_reCaptcha
```

### Configure reCAPTCHA
- Register your application with Google reCAPTCHA to obtain the **Site Key** and **Secret Key**.
- Update the `appsettings.json` file with your reCAPTCHA keys:
  ```json
  "CaptchaSettings": {
    "SiteKey": "your_site_key",
    "SecretKey": "your_secret_key",
    "VerificationUrl": "https://www.google.com/recaptcha/api/siteverify"
  }
  ```

### Configure the Database
- The connection string for SQLite is configured in `appsettings.json`:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SampleLoginAPI.db;"
  }
  ```
- The application will create the necessary database and tables upon running.

### Run the Application
- Restore the required packages:
  ```bash
  dotnet restore
  ```
- Apply any pending migrations to set up the database schema:
  ```bash
  dotnet ef database update
  ```
- Run the application:
  ```bash
  dotnet run
  ```
- The API will be accessible at `https://localhost:5001`.

## Security Features

1. **Password Hashing**:
   - Passwords are hashed using ASP.NET Core's `PasswordHasher` for secure storage.
2. **Failed Login Attempts Tracking**:
   - Tracks failed login attempts and requires reCAPTCHA validation after a specified threshold to prevent brute force attacks.
3. **JWT Authentication**:
   - Issues JSON Web Tokens (JWT) upon successful login for secure access to protected endpoints.
4. **CAPTCHA Verification**:
   - Validates reCAPTCHA tokens with Google’s verification API to differentiate between humans and bots.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request for any enhancements or bug fixes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

By integrating Google reCAPTCHA and implementing robust security measures, this project provides a secure foundation for user authentication in .NET 8 Web API applications.


## Contact
- Name: [Muthukumar Thevar](https://www.linkedin.com/in/mak11/)
- Email: mak.thevar@outlook.com
- Portfolio: https://mak-thevar.dev
- Project Link: https://github.com/mak-thevar/SampleLoginAPIWith_reCaptcha
