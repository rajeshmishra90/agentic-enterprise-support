using System.Threading.Tasks;
using Contoso.Support.Application.DTOs;

namespace Contoso.Support.Application.Interfaces;

public interface IConversationalTriageAgent
{
    /// <summary>
    /// Processes a user's message, potentially troubleshooting or gathering prerequisites 
    /// before routing to a claim or returning an answer.
    /// </summary>
    /// <param name="userMessage">The raw text from the customer.</param>
    /// <param name="sessionId">The existing conversation Session ID (null if new conversation).</param>
    /// <returns>The structured TriageResult with intent classification.</returns>
    Task<TriageResult> ProcessUserMessageAsync(string userMessage, string? sessionId = null);
}
