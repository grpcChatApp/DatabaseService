using Grpc.Core;
using GrpcChat.Database.Clients;
using DatabaseService.Application.Users;
using DatabaseService.Services.AuthenticationServer;
using ClientDto = GrpcChat.Database.Clients;

class ClientGrpcService(ClientRequestsHandler clientRequestsHandler) : ClientService.ClientServiceBase
{
    public override async Task<ClientResponseDto> CreateClient(
        CreateClientRequest request,
        ServerCallContext context)
    {
        return await clientRequestsHandler.CreateClient(request, context.CancellationToken);
    }

    public override Task<ClientResponseDto> UpdateClient(UpdateClientRequest request, ServerCallContext context)
    {
        return clientRequestsHandler.UpdateClient(request, context.CancellationToken);
    }

    public override Task<ClientResponseDto> DeleteClient(DeleteClientRequest request, ServerCallContext context)
    {
        return clientRequestsHandler.DeleteClient(request, context.CancellationToken);
    }
}