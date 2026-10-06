using System.Text.Json;
using System.Collections.Concurrent;
using Azure.AI.Projects;
using Contoso.Support.Application.DTOs;
using Contoso.Support.Application.Interfaces;
using Contoso.Support.Domain.Enums;
using Microsoft.Agents.AI;

namespace Contoso.Support.Agents;

public class ConversationalTriageAgent : IConversationalTriageAgent
{
    private readonly AIAgent _agent;
    private readonly JsonSerializerOptions _jsonOptions;
    
    // In-memory chat history (upgrade to Redis later)
    private static readonly ConcurrentDictionary<string, AgentSession> _sessions = new();

    // The API will inject the AIProjectClient (which handles the Azure credentials)
    public ConversationalTriageAgent(AIProjectClient projectClient)
    {
        _agent = projectClient.AsAIAgent(
            model: "gpt-5.4-nano", 
            instructions: @"You are the Contoso Conversational Triage Agent. 
Your job is to ask customers troubleshooting questions before allowing them to file a warranty claim.
You must always classify the user's intent into one of the following categories:
- Unknown
- ProductQuestion
- WarrantyQuestion
- WarrantyClaim
- Troubleshooting
- StatusCheck
- Escalation
- General

You must always output your response as valid JSON matching this schema:
{
    ""intent"": ""IntentType"",
    ""confidence"": 0.95,
    ""response"": ""Your message to the user"",
    ""productName"": ""Optional product name if detected"",
    ""issueDescription"": ""Optional issue description"",
    ""needsMoreInfo"": true or false
}
Do not output any markdown formatting or extra text outside the JSON.",
            name: "TriageAgent"
        );

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<TriageResult> ProcessUserMessageAsync(string userMessage, string? sessionId = null)
    {
        sessionId ??= Guid.NewGuid().ToString();

        // 1. Reconstitute the session, or create a new one
        AgentSession? session = null;
        _sessions.TryGetValue(sessionId, out session);

        if (session == null) 
        {
            session = await _agent.CreateSessionAsync();
            _sessions[sessionId] = session;
        }

        // 2. Run the agent (The SDK handles all the background polling for us!)
        var responseMessage = await _agent.RunAsync(userMessage, session);
        var responseText = responseMessage.ToString() ?? string.Empty;
        
        // Clean up markdown block if the model outputs it
        if (responseText.StartsWith("```json"))
        {
            responseText = responseText.Replace("```json", "").Replace("```", "").Trim();
        }

        try
        {
            var result = JsonSerializer.Deserialize<TriageResult>(responseText, _jsonOptions);
            if (result != null)
            {
                return result with { SessionId = sessionId };
            }
        }
        catch (JsonException)
        {
            // Fallback if model doesn't return proper JSON
        }

        return new TriageResult
        {
            Intent = IntentType.Unknown,
            Confidence = 0.0,
            Response = responseText,
            SessionId = sessionId
        };
    }
}
