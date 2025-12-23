using PingChatApp.Common.Protos;
namespace DatabaseService.Contracts.Grpc
{
    public interface IClientRequestsHandler
    {
        public Task<ClientResponse> CreateClient(CreateClientRequest request, CancellationToken ct);
        public Task<ClientResponse> UpdateClient(UpdateClientRequest request, CancellationToken ct);
        public Task<ClientResponse> DeleteClient(DeleteClientRequest request, CancellationToken ct);
    }
}