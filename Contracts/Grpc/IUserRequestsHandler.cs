using PingChatApp.Common.Protos;
namespace DatabaseService.Contracts.Grpc
{
    public interface IUserRequestsHandler
    {
        public Task<UserResponse> CreateUser(CreateUserRequest request, CancellationToken ct);
        public Task<UserResponse> UpdateUser(UpdateUserRequest request, CancellationToken ct);
        public Task<UserResponse> DeleteUser(DeleteUserRequest request, CancellationToken ct);
    }
}