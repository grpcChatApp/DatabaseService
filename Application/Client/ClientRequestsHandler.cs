using DatabaseService.Contexts;
using DatabaseService.Contracts.Kalfka;
using DatabaseService.Data.KafkaEvents;
using DatabaseService.Data.Models;
using Common.Data.KafkaEvents;
using DatabaseService.Utilities;
using GrpcChat.Database.Clients;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using DatabaseService.Contracts.Grpc;

namespace DatabaseService.Application.Users
{
    public sealed class ClientRequestsHandler(CoreContext db, IKafkaPublisher publisher) : IClientRequestsHandler
    {
        public async Task<ClientResponseDto> CreateClient(
            CreateClientRequest request,
            CancellationToken ct)
        {
            var client = Client.Create(request.Name);

            await using var tx = await db.Database.BeginTransactionAsync(ct);

            try
            {
                db.Clients.Add(client);
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                DbExceptionHandler.Handle(ex);
            }

            await publisher.PublishAsync(
                KafkaTopics.Client.Created,
                    new ClientEvent(client.ReferenceId, KafkaTopics.Client.Created, client.Name),
                    ct
                );

            await tx.CommitAsync(ct);

            return client.ToDto();
        }

        public async Task<ClientResponseDto> UpdateClient(
            UpdateClientRequest request,
            CancellationToken ct)
        {
            var client = await db.Clients.FirstOrDefaultAsync(c => c.ReferenceId.ToString() == request.ReferenceId, ct);
            if (client is null) throw new KeyNotFoundException($"Client {request.ReferenceId} not found.");

            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != client.Name)
            {
                client.Name = request.Name;
            }

            client.UpdatedDate = DateTime.UtcNow;

            await using var tx = await db.Database.BeginTransactionAsync(ct);

            try
            {
                db.Clients.Update(client);
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                DbExceptionHandler.Handle(ex);
            }

            await publisher.PublishAsync(
                KafkaTopics.Client.Updated,
                new ClientEvent(client.ReferenceId, KafkaTopics.Client.Updated, client.ClientId),
                ct
            );

            await tx.CommitAsync(ct);

            return client.ToDto();
        }

        public async Task<ClientResponseDto> DeleteClient(
            DeleteClientRequest request,
            CancellationToken ct)
        {
            var client = await db.Clients.FirstOrDefaultAsync(c => c.ReferenceId.ToString() == request.ReferenceId, ct);
            if (client is null) throw new KeyNotFoundException($"Client {request.ReferenceId} not found.");

            await using var tx = await db.Database.BeginTransactionAsync(ct);

            try
            {
                db.Clients.Remove(client);
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                DbExceptionHandler.Handle(ex);
            }

            await publisher.PublishAsync(
                KafkaTopics.Client.Deleted,
                new ClientEvent(client.ReferenceId, KafkaTopics.Client.Deleted, client.ClientId),
                ct
            );

            await tx.CommitAsync(ct);

            return client.ToDto();
        }
    }
}
