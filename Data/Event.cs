namespace r2bw_alpha.Data
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
    }
}