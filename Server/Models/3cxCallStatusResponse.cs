namespace CrownATTime.Server.Models
{
    public class ThreeCxCallStatusResult
    {

        public Calls[] CallRecords { get; set; }

        public class Calls
        {
            public string dn { get; set; }
            public string type { get; set; }
            public Device[] devices { get; set; }
            public Participant[] participants { get; set; }
        }

        public class Device
        {
            public string dn { get; set; }
            public string device_id { get; set; }
            public string user_agent { get; set; }
        }

        public class Participant
        {
            public int id { get; set; }
            public string status { get; set; }
            public string party_caller_name { get; set; }
            public string party_dn { get; set; }
            public string party_caller_id { get; set; }
            public string device_id { get; set; }
            public string party_dn_type { get; set; }
            public bool direct_control { get; set; }
            public int callid { get; set; }
            public int legid { get; set; }
            public string dn { get; set; }
        }

    }
}
