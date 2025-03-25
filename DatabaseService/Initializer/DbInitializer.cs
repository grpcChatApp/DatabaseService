using Common.Data;
using Microsoft.AspNetCore.DataProtection;
using static Common.Constants;

namespace DatabaseService.Initializer
{
    public static class DatabaseInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            if (!context.ApiScopes.Any())
            {
                context.ApiScopes.AddRange(
                    new ApiScope { Id = (int)PermissionsEnum.None, Name = Enum.GetName(PermissionsEnum.None)! },
                    new ApiScope { Id = (int)PermissionsEnum.Read, Name = Enum.GetName(PermissionsEnum.Read)! },
                    new ApiScope { Id = (int)PermissionsEnum.Write, Name = Enum.GetName(PermissionsEnum.Write)! }
                );
            }

            if (!context.ApiResources.Any())
            {
                context.ApiResources.AddRange(
                    new ApiResource { Id = 1, Name = ServiceNames.ClientApp },
                    new ApiResource { Id = 2, Name = ServiceNames.BrokerService },
                    new ApiResource { Id = 3, Name = ServiceNames.DatabaseService },
                    new ApiResource { Id = 4, Name = ServiceNames.BackendHostService },
                    new ApiResource { Id = 5, Name = ServiceNames.AuthenticationServer }
                );
            }

            context.SaveChanges();
        }
    }
}
