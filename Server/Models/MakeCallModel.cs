namespace CrownATTime.Server.Models
{
    public class MakeCallModel
    {
        public string Destination { get; set; }
        public string Extension { get; set; }
        public int Timeout { get; set; }
    }
}
