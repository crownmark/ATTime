using System.ComponentModel.DataAnnotations;

namespace CrownATTime.Server.Models
{
    public class ActionTypeDtoResult
    {
        public int id { get; set; }

        public bool isActive { get; set; }

        public bool isSystemActionType { get; set; }

        public string name { get; set; }

        public int view { get; set; }
    }
}
