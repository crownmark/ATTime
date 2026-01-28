namespace CrownATTime.Client
{
    using CrownATTime.Server.Models.ATTime;
    using Radzen;
    using System.Text;

    public sealed class AiScenarioRunnerService
    {
        private readonly IAIChatService _ai;

        public AiScenarioRunnerService(IAIChatService ai)
        {
            _ai = ai;
        }

        public async Task<string> RunAsync(
            AiPromptConfiguration cfg,
            string note,
            CancellationToken ct = default)
        {
            if (cfg == null) throw new ArgumentNullException(nameof(cfg));

            note ??= string.Empty;

            // Always isolate single-shot runs
            var session = _ai.GetOrCreateSession();
            session.Messages.Clear();

            // Build prompts
            var systemPrompt = cfg.SystemPrompt?.Trim() ?? string.Empty;
            var userPrompt = $"{cfg.UserPrompt}:{Environment.NewLine}{note}";// Render(cfg.UserPrompt, note);

            session.AddMessage("system", systemPrompt);
            session.AddMessage("user", userPrompt);

            // 🔑 THIS is the actual call that talks to the model
            var responseBuilder = new StringBuilder();

            await foreach (var token in _ai.GetCompletionsAsync(
                               userPrompt,
                               session.Id,
                               ct,
                               //model: cfg.Model,
                               systemPrompt: systemPrompt
                               //temperature: cfg.Temperature,
                               //maxTokens: cfg.MaxTokens)
                               )
                            )
            {
                responseBuilder.Append(token);
            }

            var response = responseBuilder.ToString();

            // Optional: keep history consistent
            session.AddMessage("assistant", response);

            return response;
        }

        private static string Render(string? template, string note)
        {
            if (string.IsNullOrWhiteSpace(template))
                return note;

            return template.Replace("{{Note}}", note, StringComparison.OrdinalIgnoreCase)
                           .Trim();
        }
    }
}
