namespace CrownATTime.Server.Models
{
    public class TicketUserDefinedFieldsDtoResult
    {

        public Field[] fields { get; set; }


        public class Field
        {
            public string name { get; set; }
            public string label { get; set; }
            public string type { get; set; }
            public int length { get; set; }
            public object description { get; set; }
            public bool isRequired { get; set; }
            public bool isReadOnly { get; set; }
            public bool isQueryable { get; set; }
            public bool isReference { get; set; }
            public object referenceEntityType { get; set; }
            public bool isPickList { get; set; }
            public Picklistvalue[] picklistValues { get; set; }
            public object picklistParentValueField { get; set; }
            public string defaultValue { get; set; }
            public bool isSupportedWebhookField { get; set; }
        }

        public class Picklistvalue
        {
            public string value { get; set; }
            public string label { get; set; }
            public bool isDefaultValue { get; set; }
            public int sortOrder { get; set; }
            public string parentValue { get; set; }
            public bool isActive { get; set; }
            public bool isSystem { get; set; }
        }

    }
}
