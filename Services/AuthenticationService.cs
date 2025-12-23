using PingChatApp.Common.Protos;
using Grpc.Core;
using DatabaseService.Contexts;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace DatabaseService.Services.AuthenticationService
{
    public class AuthenticationService : AuthDataService.AuthDataServiceBase
    {
        private readonly CoreContext _db;

        public AuthenticationService(CoreContext db)
        {
            _db = db;
        }

        // Client Credentials flow
        public override async Task<ResolvedClientAuthorization> ResolveClientAuthorization(
            ResolveClientAuthRequest request,
            ServerCallContext context)
        {
            var client = await _db.Clients
                .Include(c => c.Resources)
                    .ThenInclude(r => r.Scopes)
                .FirstOrDefaultAsync(c =>
                    c.ClientId == request.ClientId && c.IsActive);

            if (client == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Client not found"));

            var response = new ResolvedClientAuthorization
            {
                ClientId = client.ClientId
            };

            foreach (var resource in client.Resources)
            {
                foreach (var scope in resource.Scopes)
                {
                    response.Scopes.Add(new ScopeResponse
                    {
                        Name = scope.Name,
                        Resource = resource.Name
                    });
                }
            }

            return response;
        }

        // Password flow
        public override async Task<ValidateUserResponse> ValidateUserCredentials(
            ValidateUserRequest request,
            ServerCallContext context)
        {
            var user = await _db.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u =>
                    u.Username == request.Username && u.IsActive);

            if (user == null)
                return new ValidateUserResponse { Success = false };

            var isValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash
            );

            return new ValidateUserResponse
            {
                Success = isValid,
                UserId = user.Id
            };
        }

        // AuthCode flow
        public async override Task<ValidateAuthCodeResponse> ValidateAuthorizationCode(
            ValidateAuthCodeRequest request,
            ServerCallContext context)
        {
            // In AuthServer - not needed 
            throw new NotImplementedException();
        }

        // Token flow
        public override Task<ValidateAccessTokenResponse> ValidateAccessToken(
            ValidateAccessTokenRequest request,
            ServerCallContext context)
        {
            // In AuthServer - not needed 
            throw new NotImplementedException();
        }
    }
}
