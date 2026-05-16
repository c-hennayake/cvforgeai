using CvForgeAI.Application.DTO.Auth;

namespace CvForgeAI.Application.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);
}