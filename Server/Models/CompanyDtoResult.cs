namespace CrownATTime.Server.Models
{
    public class CompanyDtoResult
    {

        public Item item { get; set; }

        public class Item
        {
            public int id { get; set; }
            public string additionalAddressInformation { get; set; }
            public string address1 { get; set; }
            public object address2 { get; set; }
            public string alternatePhone1 { get; set; }
            public string alternatePhone2 { get; set; }
            //public object apiVendorID { get; set; }
            //public object assetValue { get; set; }
            //public object billToCompanyLocationID { get; set; }
            public string billToAdditionalAddressInformation { get; set; }
            public string billingAddress1 { get; set; }
            public string billingAddress2 { get; set; }
            //public int billToAddressToUse { get; set; }
            //public string billToAttention { get; set; }
            //public string billToCity { get; set; }
            //public int billToCountryID { get; set; }
            //public string billToState { get; set; }
            //public string billToZipCode { get; set; }
            public string city { get; set; }
            public int classification { get; set; }
            public int companyCategoryID { get; set; }
            public string companyName { get; set; }
            public string companyNumber { get; set; }
            public int companyType { get; set; }
            //public object competitorID { get; set; }
            //public int countryID { get; set; }
            //public DateTime createDate { get; set; }
            //public int createdByResourceID { get; set; }
            //public int currencyID { get; set; }
            public string fax { get; set; }
            //public object impersonatorCreatorResourceID { get; set; }
            //public int invoiceEmailMessageID { get; set; }
            //public int invoiceMethod { get; set; }
            //public bool invoiceNonContractItemsToParentCompany { get; set; }
            //public int invoiceTemplateID { get; set; }
            public bool isActive { get; set; }
            //public bool isClientPortalActive { get; set; }
            //public bool isEnabledForComanaged { get; set; }
            //public bool isTaskFireActive { get; set; }
            //public bool isTaxExempt { get; set; }
            //public DateTime lastActivityDate { get; set; }
            //public DateTime lastTrackedModifiedDateTime { get; set; }
            //public object marketSegmentID { get; set; }
            //public int ownerResourceID { get; set; }
            //public object parentCompanyID { get; set; }
            public string phone { get; set; }
            public string postalCode { get; set; }
            //public object purchaseOrderTemplateID { get; set; }
            //public int quoteEmailMessageID { get; set; }
            //public int quoteTemplateID { get; set; }
            //public string sicCode { get; set; }
            public string state { get; set; }
            public string stockMarket { get; set; }
            public string stockSymbol { get; set; }
            public object surveyCompanyRating { get; set; }
            public string taxID { get; set; }
            //public object taxRegionID { get; set; }
            //public object territoryID { get; set; }
            public string webAddress { get; set; }
            public Userdefinedfield[] userDefinedFields { get; set; }
        }

        public class Userdefinedfield
        {
            public string name { get; set; }
            public string value { get; set; }
        }

    }
}
