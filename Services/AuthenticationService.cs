using DatabaseService.Data.Models;
using DatabaseService.Services.AuthenticationServer;
using Grpc.Core;
using DatabaseService.Contexts;
using Microsoft.EntityFrameworkCore;
namespace DatabaseService.Services.AuthenticationService
{
    public class AuthenticationService : AuthDataService.AuthDataServiceBase
    {
        private readonly CoreContext _context;

        public AuthenticationService(CoreContext context)
        {
            _context = context;
        }

        public override async Task<ApiScopesResponse> GetApiScopes(EmptyRequest request, ServerCallContext context)
        {
            var scopes = await _context.ApiScopes.ToListAsync();
            return new ApiScopesResponse
            {
                Scopes = { scopes.Select(s => new AuthenticationServer.ApiScope { Id = s.Id, Name = s.Name }) }
            };
        }

        public override async Task<ApiResourcesResponse> GetApiResources(EmptyRequest request, ServerCallContext context)
        {
            var resources = await _context.ApiResources.Include(r => r.Scopes).ToListAsync();
            return new ApiResourcesResponse
            {
                Resources = {
                    resources.Select(r => new AuthenticationServer.ApiResource
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ScopeIds = { r.Scopes.Select(s => s.Id) }
                    })
                }
            };
        }

        public override async Task<ClientsResponse> GetClients(EmptyRequest request, ServerCallContext context)
        {
            var clients = await _context.Clients.Include(c => c.AllowedScopes).ToListAsync();
            return new ClientsResponse
            {
                Clients = {
                    clients.Select(c => new AuthenticationServer.Client
                    {
                        Id = c.Id,
                        ClientId = c.ClientId,
                        AllowedScopeIds = { c.AllowedScopes.Select(s => s.Id) }
                    })
                }
            };
        }
    }

}
