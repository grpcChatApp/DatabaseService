using Grpc.Core;
using PingChatApp.Common.Protos;
using DatabaseService.Application.Users;

class UserGrpcService(UserRequestHandler userRequestsHandler) : UserDataService.UserDataServiceBase
{
    public override async Task<UserDto> CreateUser(
        CreateUserRequest request,
        ServerCallContext context)
    {
        return await userRequestsHandler.CreateUser(request, context.CancellationToken);
    }

    public override Task<UserDto> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<UserDto> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}