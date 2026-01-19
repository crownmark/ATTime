namespace CrownATTime.Server.Models
{
    public class TimeEntryDtoResult
    {


        public class Item
        {
            public int id { get; set; }
            public DateTime billingApprovalDateTime { get; set; }
            public int billingApprovalLevelMostRecent { get; set; }
            public int billingApprovalResourceID { get; set; }
            public int billingCodeID { get; set; }
            public int contractID { get; set; }
            public int contractServiceBundleID { get; set; }
            public int contractServiceID { get; set; }
            public DateTime createDateTime { get; set; }
            public int creatorUserID { get; set; }
            public DateTime dateWorked { get; set; }
            public DateTime endDateTime { get; set; }
            public int hoursToBill { get; set; }
            public int hoursWorked { get; set; }
            public int impersonatorCreatorResourceID { get; set; }
            public int impersonatorUpdaterResourceID { get; set; }
            public int internalBillingCodeID { get; set; }
            public string internalNotes { get; set; }
            public bool isInternalNotesVisibleToComanaged { get; set; }
            public bool isNonBillable { get; set; }
            public DateTime lastModifiedDateTime { get; set; }
            public int lastModifiedUserID { get; set; }
            public int offsetHours { get; set; }
            public int resourceID { get; set; }
            public int roleID { get; set; }
            public bool showOnInvoice { get; set; }
            public DateTime startDateTime { get; set; }
            public string summaryNotes { get; set; }
            public int taskID { get; set; }
            public int ticketID { get; set; }
            public int timeEntryType { get; set; }
        }

    }
}
