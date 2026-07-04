namespace GestionMicrofinance.Application.Auth;

public interface IAuthService
{
    Task<PersonneResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
