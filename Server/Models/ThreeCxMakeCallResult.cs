namespace CrownATTime.Server.Models
{
    public class ThreeCxMakeCallResult
    {

        public Calls[] CallRecords { get; set; }


        public class Calls
        {
            public string finalstatus { get; set; }
            public string reason { get; set; }
            public Result result { get; set; }
            public string reasontext { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string status { get; set; }
            public string dn { get; set; }
            public string party_caller_name { get; set; }
            public string party_dn { get; set; }
            public string party_caller_id { get; set; }
            public string party_did { get; set; }
            public string device_id { get; set; }
            public string party_dn_type { get; set; }
            public bool direct_control { get; set; }
            public string originated_by_dn { get; set; }
            public string originated_by_type { get; set; }
            public string referred_by_dn { get; set; }
            public string referred_by_type { get; set; }
            public string on_behalf_of_dn { get; set; }
            public string on_behalf_of_type { get; set; }
            public int callid { get; set; }
            public int legid { get; set; }
        }

    }
}
