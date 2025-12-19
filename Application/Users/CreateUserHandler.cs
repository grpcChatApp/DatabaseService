using System.Threading;
using System.Threading.Tasks;
using DatabaseService.Contexts;
using DatabaseService.Data.KafkaEvents;
using DatabaseService.Data.Models;
using GrpcChat.Database.Users;

namespace DatabaseService.Application.Users
{
    public sealed class CreateUserHandler(CoreContext db, IKafkaPublisher publisher)
    {
        public async Task<UserDto> Handle(
            CreateUserRequest request,
            CancellationToken ct)
        {
            var user = User.Create(request.Username, request.Email, request.Name, request.PasswordHash);

            await using var tx = await db.Database.BeginTransactionAsync(ct);

            db.Users.Add(user);
            await db.SaveChangesAsync(ct);

            await publisher.PublishAsync(
                KafkaTopics.User.Created,
                new UserEvent(user.ReferenceId.ToString(), KafkaTopics.User.Created, user.Username),
                ct
            );

            await tx.CommitAsync(ct);

            return user.ToDto();
        }
    }
}
