using DatabaseService.Contexts;
using DatabaseService.Data.Models.Auth;
using DatabaseService.Data.Models;
using static Common.Constants;

namespace DatabaseService.Data
{
    public static class Initializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CoreContext>();

            EnsureScope(context, "db.read", PermissionLevel.Read);
            EnsureScope(context, "db.write", PermissionLevel.Write);
            EnsureScope(context, "kafka.publish", PermissionLevel.Write);
            EnsureScope(context, "chat.read", PermissionLevel.Read);
            EnsureScope(context, "chat.write", PermissionLevel.Write);
            EnsureScope(context, "host.users.create", PermissionLevel.Admin);
            context.SaveChanges();
            // if (!context.ApiScopes.Any())
            // {
            //     context.ApiScopes.AddRange(
            //         new ApiScope { Name = "db.read", Level = PermissionLevel.Read },
            //         new ApiScope { Name = "db.write", Level = PermissionLevel.Write },
            //         new ApiScope { Name = "kafka.publish", Level = PermissionLevel.Write },

            //         new ApiScope { Name = "chat.read", Level = PermissionLevel.Read },
            //         new ApiScope { Name = "chat.write", Level = PermissionLevel.Write },

            //         new ApiScope { Name = "host.users.create", Level = PermissionLevel.Admin }
            //     );

            //     context.SaveChanges();
            // }

            if (!context.ApiResources.Any())
            {
                context.ApiResources.AddRange(
                    new ApiResource { Id = 1, Name = ProtectedServices.ClientApp },
                    new ApiResource { Id = 2, Name = ProtectedServices.BrokerService },
                    new ApiResource { Id = 3, Name = ProtectedServices.DatabaseService },
                    new ApiResource { Id = 4, Name = ProtectedServices.HostApp },
                    new ApiResource { Id = 5, Name = ProtectedServices.AuthService }
                );
                context.SaveChanges();
            }

            if (!context.ResourceScopeMappings.Any())
            {
                var readScope = context.ApiScopes.First(s => s.Name == "db.read");
                var writeScope = context.ApiScopes.First(s => s.Name == "db.write");
                var publishScope = context.ApiScopes.First(s => s.Name == "kafka.publish");

                var clientAppResource = context.ApiResources.First(r => r.Name == ProtectedServices.ClientApp);
                var brokerResource = context.ApiResources.First(r => r.Name == ProtectedServices.BrokerService);
                var databaseResource = context.ApiResources.First(r => r.Name == ProtectedServices.DatabaseService);

                context.ResourceScopeMappings.AddRange(
                    new ResourceScopeMapping { ResourceId = clientAppResource.Id, Name = "clientApp-read", Resource = clientAppResource, ScopeId = readScope.Id, Scope = readScope },
                    new ResourceScopeMapping { ResourceId = clientAppResource.Id, Name = "clientApp-write", Resource = clientAppResource, ScopeId = writeScope.Id, Scope = writeScope },
                    new ResourceScopeMapping { ResourceId = brokerResource.Id, Name = "broker-publish", Resource = brokerResource, ScopeId = publishScope.Id, Scope = publishScope },
                    new ResourceScopeMapping { ResourceId = databaseResource.Id, Name = "database-read", Resource = databaseResource, ScopeId = readScope.Id, Scope = readScope },
                    new ResourceScopeMapping { ResourceId = databaseResource.Id, Name = "database-write", Resource = databaseResource, ScopeId = writeScope.Id, Scope = writeScope }
                );

                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                var hostClient = Client.Create("Host Application", ProtectedServices.HostApp, ClientType.Confidential);
                context.Clients.Add(hostClient);
                var databaseApi = context.ApiResources.Single(r => r.Name == ProtectedServices.DatabaseService);

                context.ClientResourceMappings.Add(
                    new ClientResourceMapping
                    {
                        ClientId = hostClient.Id,
                        Client = hostClient,
                        ResourceId = databaseApi.Id,
                        Resource = databaseApi
                    }
                );

                context.SaveChanges();
            }
        }

        private static void EnsureScope(CoreContext context, string name, PermissionLevel level)
        {
            if (!context.ApiScopes.Any(s => s.Name == name))
            {
                context.ApiScopes.Add(new ApiScope
                {
                    Name = name,
                    Level = level
                });
            }
        }
    }
}
