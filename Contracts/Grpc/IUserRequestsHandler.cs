using PingChatApp.Common.Protos;
namespace DatabaseService.Contracts.Grpc
{
    public interface IUserRequestsHandler
    {
        public Task<UserDto> CreateUser(CreateUserRequest request, CancellationToken ct);
        public Task<UserDto> UpdateUser(UpdateUserRequest request, CancellationToken ct);
        public Task<UserDto> DeleteUser(DeleteUserRequest request, CancellationToken ct);
    }
}