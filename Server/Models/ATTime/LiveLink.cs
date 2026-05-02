using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("LiveLinks", Schema = "dbo")]
    public partial class LiveLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LiveLinkId { get; set; }

        public bool Active { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(4000)]
        public string AssignedTo { get; set; }

        public bool ShareWithOthers { get; set; }

        [Required]
        [MaxLength(500)]
        public string Url { get; set; }

        [Required]
        [MaxLength(50)]
        public string HttpMethod { get; set; }

        public bool RequiresConfirmationToRun { get; set; }

        [MaxLength(50)]
        public string Icon { get; set; }

        [MaxLength(50)]
        public string DialogWindowWidth { get; set; }

        public bool ShowInNextActions { get; set; }
    }
}