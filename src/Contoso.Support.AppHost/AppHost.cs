var builder = DistributedApplication.CreateBuilder(args);

// --- API (The Front Door) ---
var api = builder.AddProject<Projects.Contoso_Support_Api>("contoso-api")
    .WithExternalHttpEndpoints();

builder.Build().Run();
