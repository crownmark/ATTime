using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models
{
    public class ContractDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("billingPreference")]
        public int BillingPreference { get; set; }

        [JsonPropertyName("billToCompanyContactID")]
        public long? BillToCompanyContactID { get; set; }

        [JsonPropertyName("billToCompanyID")]
        public long? BillToCompanyID { get; set; }

        [JsonPropertyName("companyID")]
        public long CompanyID { get; set; }

        [JsonPropertyName("contactID")]
        public long? ContactID { get; set; }

        [JsonPropertyName("contactName")]
        public string? ContactName { get; set; }

        [JsonPropertyName("contractCategory")]
        public int? ContractCategory { get; set; }

        [JsonPropertyName("contractExclusionSetID")]
        public long? ContractExclusionSetID { get; set; }

        [JsonPropertyName("contractName")]
        public string ContractName { get; set; } = string.Empty;

        [JsonPropertyName("contractNumber")]
        public string? ContractNumber { get; set; }

        [JsonPropertyName("contractPeriodType")]
        public int? ContractPeriodType { get; set; }

        [JsonPropertyName("contractType")]
        public int ContractType { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonPropertyName("estimatedCost")]
        public decimal? EstimatedCost { get; set; }

        [JsonPropertyName("estimatedHours")]
        public decimal? EstimatedHours { get; set; }

        [JsonPropertyName("estimatedRevenue")]
        public decimal? EstimatedRevenue { get; set; }

        [JsonPropertyName("exclusionContractID")]
        public long? ExclusionContractID { get; set; }

        [JsonPropertyName("internalCurrencyOverageBillingRate")]
        public decimal? InternalCurrencyOverageBillingRate { get; set; }

        [JsonPropertyName("internalCurrencySetupFee")]
        public decimal? InternalCurrencySetupFee { get; set; }

        [JsonPropertyName("isCompliant")]
        public bool? IsCompliant { get; set; }

        [JsonPropertyName("isDefaultContract")]
        public bool? IsDefaultContract { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonPropertyName("opportunityID")]
        public long? OpportunityID { get; set; }

        [JsonPropertyName("organizationalLevelAssociationID")]
        public long? OrganizationalLevelAssociationID { get; set; }

        [JsonPropertyName("overageBillingRate")]
        public decimal? OverageBillingRate { get; set; }

        [JsonPropertyName("purchaseOrderNumber")]
        public string? PurchaseOrderNumber { get; set; }

        [JsonPropertyName("renewedContractID")]
        public long? RenewedContractID { get; set; }

        [JsonPropertyName("serviceLevelAgreementID")]
        public int? ServiceLevelAgreementID { get; set; }

        [JsonPropertyName("setupFee")]
        public decimal? SetupFee { get; set; }

        [JsonPropertyName("setupFeeBillingCodeID")]
        public long? SetupFeeBillingCodeID { get; set; }

        [JsonPropertyName("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        // JSON: 0 / 1
        [JsonPropertyName("timeReportingRequiresStartAndStopTimes")]
        public int TimeReportingRequiresStartAndStopTimes { get; set; }

        [JsonPropertyName("userDefinedFields")]
        public List<UserDefinedFieldDto> UserDefinedFields { get; set; } = new();
    }

    public class UserDefinedFieldDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public object? Value { get; set; }  // can be string/number/null depending on UDF
    }
}
