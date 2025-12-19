using Grpc.Core;
using GrpcChat.Database.Users;
using DatabaseService.Application.Users;

class UserGrpcService(CreateUserHandler createUserHandler) : UserService.UserServiceBase
{
    public override async Task<UserDto> CreateUser(
        CreateUserRequest request,
        ServerCallContext context)
    {
        return await createUserHandler.Handle(request, context.CancellationToken);
    }
}