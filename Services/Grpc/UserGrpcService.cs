using Grpc.Core;
using PingChatApp.Common.Protos;
using DatabaseService.Application.Users;

class UserGrpcService(UserRequestHandler userRequestsHandler) : UserDataService.UserDataServiceBase
{
    public override async Task<UserResponse> CreateUser(
        CreateUserRequest request,
        ServerCallContext context)
    {
        return await userRequestsHandler.CreateUser(request, context.CancellationToken);
    }

    public override Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<UserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}