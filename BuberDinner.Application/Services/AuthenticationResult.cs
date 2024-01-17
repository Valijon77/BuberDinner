using BuberDinner.Domain;

namespace BuberDinner.Application.Services
{
    public record AuthenticationResult(
        User User,
        string Token
    );
}
