namespace CrownATTime.Server.Models
{
    using System.Text.Json.Serialization;
    public record LookupItemDto(int Value, string Label);

    //public class BillingCodeDto
    //{
    //    [JsonPropertyName("id")]
    //    public int Id { get; set; }

    //    [JsonPropertyName("description")]
    //    public string Description { get; set; } = string.Empty;

    //    [JsonPropertyName("isActive")]
    //    public bool IsActive { get; set; }
    //}

    //public class RoleDto
    //{
    //    [JsonPropertyName("id")]
    //    public int Id { get; set; }

    //    [JsonPropertyName("name")]
    //    public string Name { get; set; } = string.Empty;

    //    [JsonPropertyName("isActive")]
    //    public bool IsActive { get; set; }
    //}

    //public class ContractDto
    //{
    //    [JsonPropertyName("id")]
    //    public long Id { get; set; }

    //    [JsonPropertyName("contractName")]
    //    public string ContractName { get; set; } = string.Empty;

    //    [JsonPropertyName("companyID")]
    //    public long CompanyId { get; set; }

    //    [JsonPropertyName("isActive")]
    //    public bool IsActive { get; set; }
    //}
}
