using System.ComponentModel.DataAnnotations.Schema;

namespace CrownATTime.Server.Models.ATTime
{


    public partial class TimeEntry
    {
        //[NotMapped]
        //public double TimerHoursWorked =>
        //    (DurationMs ?? 0) / 3_600_000d; // 1000 * 60 * 60

        //[NotMapped]
        //public string ElapsedFormatted =>
        //    TimeSpan.FromMilliseconds(DurationMs ?? 0).ToString(@"hh\:mm\:ss");
    }

}
