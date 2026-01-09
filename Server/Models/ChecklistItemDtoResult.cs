namespace CrownATTime.Server.Models
{
    public class ChecklistItemDtoResult
    {
        public Item item { get; set; }

        public class Item
        {
            public int id { get; set; }

            public bool isCompleted { get; set; }
            public bool isImportant { get; set; }
            public string itemName { get; set; }
            public int? knowledgebaseArticleID { get; set; }
            public int? position { get; set; }
            public int ticketID { get; set; }
        }
        
    }
}
