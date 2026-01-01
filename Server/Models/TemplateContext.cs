using CrownATTime.Server.Models.ATTime;

namespace CrownATTime.Server.Models
{
    public sealed class TemplateContext
    {
        public ContactDtoResult.Item? Contact { get; init; }
        public TicketDtoResult.Item? Ticket { get; init; }
        public ResourceCache? Resource { get; init; }

        // PicklistName -> (numericValue -> displayLabel)
        public IReadOnlyDictionary<string, IReadOnlyDictionary<int, string>>? Picklists { get; init; }

        // handy extras
        public DateTimeOffset Now { get; init; } = DateTimeOffset.Now;
        public string BaseUrl { get; init; } = "";
    }
}
