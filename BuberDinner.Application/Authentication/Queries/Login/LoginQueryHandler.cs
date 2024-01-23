using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Users;

namespace BuberDinner.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository
        )
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(
            LoginQuery query,
            CancellationToken cancellationToken
        )
        {
            // 1. Validate user exists
            if (_userRepository.GetUserByEmail(query.Email) is not User user)
            {
                return await Task.FromResult(Errors.Authentication.InvalidCredentials);
            }

            // 2. Password is correct
            if (user.Password != query.Password)
            {
                return await Task.FromResult(new[] { Errors.Authentication.InvalidCredentials });
            }

            // 3. Create JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return await Task.FromResult(new AuthenticationResult(user, token));
        }
    }
}
