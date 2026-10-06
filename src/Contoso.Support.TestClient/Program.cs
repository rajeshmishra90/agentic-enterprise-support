using Azure.Identity;
using Azure.AI.Projects;
using Contoso.Support.Agents;

Console.WriteLine("Connecting to Azure AI Foundry (gpt-5.4-nano) using your az login credentials...");

var connectionString = "https://contosov2-foundry.services.ai.azure.com/api/projects/contosov2-ai-foundry-project"; 
var projectClient = new AIProjectClient(new Uri(connectionString), new DefaultAzureCredential());

var agent = new ConversationalTriageAgent(projectClient);

Console.WriteLine("\nUser: My laptop screen is flickering.");
Console.WriteLine("Thinking...");

var response = await agent.ProcessUserMessageAsync("My laptop screen is flickering.");

Console.WriteLine($"\nTriage Agent: {response}");
