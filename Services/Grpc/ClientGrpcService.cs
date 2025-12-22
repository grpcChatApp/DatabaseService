using Grpc.Core;
using PingChatApp.Common.Protos;
using DatabaseService.Application.Users;
using ClientDto = PingChatApp.Common.Protos;

class ClientGrpcService(ClientRequestsHandler clientRequestsHandler) : ClientDataService.ClientDataServiceBase
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