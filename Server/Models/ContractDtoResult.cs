namespace CrownATTime.Server.Models
{
    public class ContractDtoResult
    {

        public Item item { get; set; }


        public class Item
        {
            public int id { get; set; }
            public int billingPreference { get; set; }
            //public object billToCompanyContactID { get; set; }
            //public object billToCompanyID { get; set; }
            public int companyID { get; set; }
            //public object contactID { get; set; }
            //public object contactName { get; set; }
            //public int contractCategory { get; set; }
            //public object contractExclusionSetID { get; set; }
            public string contractName { get; set; }
            //public object contractNumber { get; set; }
            //public int contractPeriodType { get; set; }
            //public int contractType { get; set; }
            public string description { get; set; }
            public DateTime endDate { get; set; }
            //public float estimatedCost { get; set; }
            //public float estimatedHours { get; set; }
            //public float estimatedRevenue { get; set; }
            //public object exclusionContractID { get; set; }
            //public object internalCurrencyOverageBillingRate { get; set; }
            //public float internalCurrencySetupFee { get; set; }
            //public bool isCompliant { get; set; }
            public bool? isDefaultContract { get; set; }
            //public DateTime lastModifiedDateTime { get; set; }
            //public object opportunityID { get; set; }
            //public int organizationalLevelAssociationID { get; set; }
            //public object overageBillingRate { get; set; }
            //public string purchaseOrderNumber { get; set; }
            //public object renewedContractID { get; set; }
            //public int serviceLevelAgreementID { get; set; }
            //public float setupFee { get; set; }
            //public object setupFeeBillingCodeID { get; set; }
            //public DateTime startDate { get; set; }
            public int status { get; set; }
            //public int timeReportingRequiresStartAndStopTimes { get; set; }
            public object[] userDefinedFields { get; set; }
        }

    }
}
