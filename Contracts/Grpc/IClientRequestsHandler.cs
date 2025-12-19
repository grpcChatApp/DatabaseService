using GrpcChat.Database.Clients;
namespace DatabaseService.Contracts.Grpc
{
    public interface IClientRequestsHandler
    {
        public Task<ClientResponseDto> CreateClient(CreateClientRequest request, CancellationToken ct);
        public Task<ClientResponseDto> UpdateClient(UpdateClientRequest request, CancellationToken ct);
        public Task<ClientResponseDto> DeleteClient(DeleteClientRequest request, CancellationToken ct);
    }
}