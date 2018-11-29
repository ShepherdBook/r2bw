namespace r2bw_alpha.Data
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set;}

        public ICollection<Attendance> Attendance { get; set; }
    }
}