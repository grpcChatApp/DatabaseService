using PingChatApp.Common.Protos;
using Grpc.Core;
using DatabaseService.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
namespace DatabaseService.Services.AuthenticationService
{
    public class AuthenticationService : AuthDataService.AuthDataServiceBase
    {
        private readonly CoreContext _context;

        public AuthenticationService(CoreContext context)
        {
            _context = context;
        }

        public override async Task<ApiScopesResponseDto> GetApiScopes(EmptyRequestDto request, ServerCallContext context)
        {
            var scopes = await _context.ApiScopes.ToListAsync();
            return new ApiScopesResponseDto
            {
                Scopes = { scopes.Select(s => new ApiScopeDto { Id = s.Id, Name = s.Name }) }
            };
        }

        public override async Task<ApiResourcesResponseDto> GetApiResources(EmptyRequestDto request, ServerCallContext context)
        {
            var resources = await _context.ApiResources.Include(r => r.Scopes).ToListAsync();

            return new ApiResourcesResponseDto
            {
                Resources = {
                    resources.Select(r => new ApiResourceDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ScopeIds = { r.Scopes.Select(s => s.Id) }
                    })
                }
            };
        }

        public override async Task<ClientsResponseDto> GetClients(EmptyRequestDto request, ServerCallContext context)
        {
            var clients = await _context.Clients.Include(c => c.Scopes).ToListAsync();
            return new ClientsResponseDto
            {
                Clients = {
                    clients.Select(c => new ClientDto
                    {
                        Id = c.Id,
                        ClientId = c.ClientId,
                        AllowedScopeIds = { c.Scopes.Select(s => s.Id) }
                    })
                }
            };
        }
    }

}
