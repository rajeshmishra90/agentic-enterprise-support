using System;
using System.Threading;
using System.Threading.Tasks;
using Contoso.Support.Domain.Entities;

namespace Contoso.Support.Application.Interfaces;

/// <summary>
/// Service for managing conversation history and persistence.
/// </summary>
public interface IConversationService
{
    Task<Conversation> GetConversationAsync(Guid conversationId, CancellationToken ct = default);
    Task<Conversation> CreateConversationAsync(Guid customerId, CancellationToken ct = default);
    Task AddMessageAsync(Guid conversationId, ConversationMessage message, CancellationToken ct = default);
}
