using PostMVC.Models;

namespace PostMVC.Data.Service;

public interface IAuthService
{
    Task<bool> Register(RegisterViewModel model);
    Task<string?> Login(LoginViewModel model);
}
