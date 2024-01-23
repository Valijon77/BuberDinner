using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Users;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository
        )
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(
            RegisterCommand command,
            CancellationToken cancellationToken
        )
        {
            // 1. Validate user does not exist
            if (_userRepository.GetUserByEmail(command.Email) is not null)
            {
                return await Task.FromResult(Errors.User.DuplicateEmail);
            }

            // 2. Create user (generate unique id) & Persist user to DB
            var user = User.Create(command.FirstName, command.LastName, command.Email, command.Password);

            _userRepository.Add(user);

            // 3. Create jwt token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return await Task.FromResult(new AuthenticationResult(user, token));
        }
    }
}
