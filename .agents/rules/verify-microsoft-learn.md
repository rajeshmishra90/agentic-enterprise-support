# Microsoft Learn Verification Rule

**Applies to:** All code generation tasks (C#, Bicep, SDK usage, etc.)

## Rule Definition
Before generating any new code, you MUST:
1. Use the `search_web` tool to search `learn.microsoft.com` for the specific service or SDK being used.
2. Verify the absolute latest recommended syntax, package names, and best practices.
3. Explicitly state in your response what you found on Microsoft Learn before presenting the code to the user.

**Reason:** Microsoft AI SDKs (like MAF, Azure AI Projects, and Azure AI Foundry) evolve rapidly. We cannot rely on training data cutoffs. We must ensure the repository uses the finalized, modern syntax.
