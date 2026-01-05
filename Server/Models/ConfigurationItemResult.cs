namespace CrownATTime.Server.Models
{
    public class ConfigurationItemResult
    {

        public Item item { get; set; }


        public class Item
        {
            public int id { get; set; }
            public int apiVendorID { get; set; }
            public int configurationItemCategoryID { get; set; }
            public int companyID { get; set; }
            public int? companyLocationID { get; set; }
            public int configurationItemType { get; set; }
            public int? contactID { get; set; }
            //public object contractID { get; set; }
            //public object contractServiceBundleID { get; set; }
            //public object contractServiceID { get; set; }
            public DateTime createDate { get; set; }
            public int createdByPersonID { get; set; }
            //public object dailyCost { get; set; }
            //public object dattoAvailableKilobytes { get; set; }
            //public object dattoDeviceMemoryMegabytes { get; set; }
            //public object dattoDrivesErrors { get; set; }
            //public object dattoHostname { get; set; }
            //public object dattoInternalIP { get; set; }
            //public object dattoKernelVersionID { get; set; }
            //public object dattoLastCheckInDateTime { get; set; }
            //public object dattoNICSpeedKilobitsPerSecond { get; set; }
            //public object dattoNumberOfAgents { get; set; }
            //public object dattoNumberOfDrives { get; set; }
            //public object dattoNumberOfVolumes { get; set; }
            //public object dattoOffsiteUsedBytes { get; set; }
            //public object dattoOSVersionID { get; set; }
            //public object dattoPercentageUsed { get; set; }
            //public object dattoProtectedKilobytes { get; set; }
            //public object dattoRemoteIP { get; set; }
            //public object dattoSerialNumber { get; set; }
            //public object dattoUptimeSeconds { get; set; }
            //public object dattoUsedKilobytes { get; set; }
            //public object dattoZFSVersionID { get; set; }
            //public object deviceNetworkingID { get; set; }
            //public object domain { get; set; }
            //public object domainRegistrarID { get; set; }
            //public object domainRegistrationDateTime { get; set; }
            //public object domainLastUpdatedDateTime { get; set; }
            //public object domainExpirationDateTime { get; set; }
            //public object hourlyCost { get; set; }
            //public object impersonatorCreatorResourceID { get; set; }
            public DateTime installDate { get; set; }
            //public object installedByContactID { get; set; }
            public int installedByID { get; set; }
            public bool isActive { get; set; }
            public int lastActivityPersonID { get; set; }
            public int lastActivityPersonType { get; set; }
            public DateTime lastModifiedTime { get; set; }
            public string location { get; set; }
            //public object monthlyCost { get; set; }
            //public object notes { get; set; }
            //public object numberOfUsers { get; set; }
            //public object parentConfigurationItemID { get; set; }
            //public object perUseCost { get; set; }
            public int productID { get; set; }
            public string referenceNumber { get; set; }
            public string referenceTitle { get; set; }
            public int rmmDeviceAuditAntivirusStatusID { get; set; }
            public int rmmDeviceAuditArchitectureID { get; set; }
            //public object rmmDeviceAuditBackupStatusID { get; set; }
            public string rmmDeviceAuditDescription { get; set; }
            public int rmmDeviceAuditDeviceTypeID { get; set; }
            public int rmmDeviceAuditDisplayAdaptorID { get; set; }
            public int rmmDeviceAuditDomainID { get; set; }
            public string rmmDeviceAuditExternalIPAddress { get; set; }
            public string rmmDeviceAuditHostname { get; set; }
            public string rmmDeviceAuditIPAddress { get; set; }
            public string rmmDeviceAuditLastUser { get; set; }
            public string rmmDeviceAuditMacAddress { get; set; }
            public int rmmDeviceAuditManufacturerID { get; set; }
            //public object rmmDeviceAuditFirmwareID { get; set; }
            public long rmmDeviceAuditMemoryBytes { get; set; }
            public int rmmDeviceAuditMissingPatchCount { get; set; }
            //public object rmmDeviceAuditMobileNetworkOperatorID { get; set; }
            public string rmmDeviceAuditMobileNumber { get; set; }
            public int rmmDeviceAuditModelID { get; set; }
            public int rmmDeviceAuditMotherboardID { get; set; }
            public string rmmDeviceAuditOperatingSystem { get; set; }
            public int rmmDeviceAuditPatchStatusID { get; set; }
            public int rmmDeviceAuditProcessorID { get; set; }
            public int rmmDeviceAuditServicePackID { get; set; }
            public string rmmDeviceAuditSNMPContact { get; set; }
            public string rmmDeviceAuditSNMPLocation { get; set; }
            public string rmmDeviceAuditSNMPName { get; set; }
            public int rmmDeviceAuditSoftwareStatusID { get; set; }
            public long rmmDeviceAuditStorageBytes { get; set; }
            public int rmmDeviceID { get; set; }
            public string rmmDeviceUID { get; set; }
            public string rmmVsaxDeviceID { get; set; }
            public int rmmOpenAlertCount { get; set; }
            public string serialNumber { get; set; }
            //public object serviceBundleID { get; set; }
            //public object serviceID { get; set; }
            //public object serviceLevelAgreementID { get; set; }
            //public object setupFee { get; set; }
            //public object sourceChargeID { get; set; }
            //public object sourceChargeType { get; set; }
            //public object sslSource { get; set; }
            //public object sslCommonName { get; set; }
            //public object sslValidFromDateTime { get; set; }
            //public object sslValidUntilDateTime { get; set; }
            //public object sslIssuedBy { get; set; }
            //public object sslOrganization { get; set; }
            //public object sslOrganizationUnit { get; set; }
            //public object sslLocation { get; set; }
            //public object sslSerialNumber { get; set; }
            //public object sslSignatureAlgorithm { get; set; }
            //public object vendorID { get; set; }
            public DateTime? warrantyExpirationDate { get; set; }
            public bool rmmIsInMaintenanceMode { get; set; }
            public bool rmmIsMobileDeviceManagementEnrolled { get; set; }
            public string rmmDeviceUrl { get; set; }
            public Userdefinedfield[] userDefinedFields { get; set; }
        }

        public class Userdefinedfield
        {
            public string name { get; set; }
            public string value { get; set; }
        }

    }
}
