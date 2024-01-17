using ErrorOr;

namespace BuberDinner.Application.Services
{
    public interface IAuthenticationService
    {
        ErrorOr<AuthenticationResult> Login(string email, string password);
        ErrorOr<AuthenticationResult> Register(
            string firstName,
            string lastName,
            string email,
            string password
        );
    }
}