using Microsoft.EntityFrameworkCore;
using DatabaseService.Contexts;
using DatabaseService.Data.KafkaEvents;
using DatabaseService.Data.Models;
using GrpcChat.Database.Users;
using Npgsql;
using DatabaseService.Utilities;
using DatabaseService.Contracts.Grpc;
using DatabaseService.Contracts.Kalfka;
using Grpc.Core;

namespace DatabaseService.Application.Users
{
    public sealed class UserRequestHandler(CoreContext db, IKafkaPublisher publisher) : IUserRequestsHandler
    {
        public async Task<UserDto> CreateUser(
            CreateUserRequest request,
            CancellationToken ct)
        {
            var user = User.Create(request.Username, request.Email, request.Name, request.PasswordHash);
            await using var tx = await db.Database.BeginTransactionAsync(ct);
            try
            {
                var roleGuids = request.Roles.Select(Guid.Parse).ToList();
                if (roleGuids.Count == 0)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "At least one role must be assigned to the user."));
                }
                
                var roles = await db.Roles
                    .Where(r => roleGuids.Contains(r.ReferenceId))
                    .ToListAsync(ct); 
                user.Roles = roles;

                db.Users.Add(user);
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                DbExceptionHandler.Handle(ex);
            }

            await publisher.PublishAsync(
                KafkaTopics.User.Created,
                new UserEvent(user.ReferenceId, KafkaTopics.User.Created, user.Username),
                ct
            );

            await tx.CommitAsync(ct);

            return user.ToDto();
        }

        public async Task<UserDto> UpdateUser(
            UpdateUserRequest request,
            CancellationToken ct)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.ReferenceId.ToString() == request.ReferenceId, ct);
            if (user is null) throw new KeyNotFoundException($"User {request.ReferenceId} not found.");

            if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != user.Username)
            {
                var exists = await db.Users.AnyAsync(u => u.Username == request.Username && u.ReferenceId != user.ReferenceId, ct);
                if (exists) throw new InvalidOperationException($"Username '{request.Username}' is already taken."); // map to 409 in outer layer
                user.Username = request.Username;
            }

            if (!string.IsNullOrWhiteSpace(request.Email)) user.Email = request.Email;
            if (!string.IsNullOrWhiteSpace(request.Name)) user.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.PasswordHash)) user.PasswordHash = request.PasswordHash;

            user.UpdatedDate = DateTime.UtcNow;

            await using var tx = await db.Database.BeginTransactionAsync(ct);

            try
            {
                db.Users.Update(user);
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                DbExceptionHandler.Handle(ex);
            }

            await publisher.PublishAsync(
                KafkaTopics.User.Updated,
                new UserEvent(user.ReferenceId, KafkaTopics.User.Updated, user.Username),
                ct
            );

            await tx.CommitAsync(ct);

            return user.ToDto();
        }

        public async Task<UserDto> DeleteUser(
            DeleteUserRequest request,
            CancellationToken ct)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.ReferenceId.ToString() == request.ReferenceId, ct);
            if (user is null) throw new KeyNotFoundException($"User {request.ReferenceId} not found.");

            await using var tx = await db.Database.BeginTransactionAsync(ct);

            try
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                DbExceptionHandler.Handle(ex);
            }

            await publisher.PublishAsync(
                KafkaTopics.User.Deleted,
                new UserEvent(user.ReferenceId, KafkaTopics.User.Deleted, user.Username),
                ct
            );

            await tx.CommitAsync(ct);

            return user.ToDto();
        }
    }
}
