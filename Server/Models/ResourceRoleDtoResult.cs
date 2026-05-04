namespace CrownATTime.Server.Models
{
    public class ResourceRoleDtoResult
    {

        public Item[] items { get; set; }
        public Pagedetails pageDetails { get; set; }

        public class Pagedetails
        {
            public int count { get; set; }
            public int requestCount { get; set; }
            public object prevPageUrl { get; set; }
            public object nextPageUrl { get; set; }
        }

        public class Item
        {
            public long id { get; set; }
            public int? departmentID { get; set; }
            public bool isActive { get; set; }
            public int? queueID { get; set; }
            public int resourceID { get; set; }
            public int roleID { get; set; }
        }


    }
}
