using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models
{
    public class TicketEntityFieldsDto
    {
        /// <summary>
        /// Root object for entityInformation/fields response.
        /// </summary>
        public class EntityInformationFieldsResponse
        {
            [JsonPropertyName("fields")]
            public List<EntityField> Fields { get; set; } = new();
        }

        public class EntityField
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("label")]
            public string Label { get; set; } = string.Empty;

            [JsonPropertyName("type")]
            public string Type { get; set; } = string.Empty;

            [JsonPropertyName("length")]
            public int Length { get; set; }

            [JsonPropertyName("precision")]
            public int Precision { get; set; }

            [JsonPropertyName("scale")]
            public int Scale { get; set; }

            [JsonPropertyName("picklistValues")]
            public List<EntityPicklistValue>? PicklistValues { get; set; }

            [JsonPropertyName("isRequired")]
            public bool? IsRequired { get; set; }

            [JsonPropertyName("isReadOnly")]
            public bool? IsReadOnly { get; set; }

            [JsonPropertyName("isReference")]
            public bool? IsReference { get; set; }

            [JsonPropertyName("referenceEntityType")]
            public string? ReferenceEntityType { get; set; }
        }

        public class EntityPicklistValue
        {
            [JsonPropertyName("value")]
            public string? Value { get; set; }

            [JsonPropertyName("label")]
            public string Label { get; set; } = string.Empty;

            [JsonPropertyName("isDefault")]
            public bool? IsDefault { get; set; }
        }
    }
}
