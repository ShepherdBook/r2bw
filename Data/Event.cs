namespace r2bw_alpha.Data
{
    using System;
    using System.Collections.Generic;

    public class Event
    {

        public Event() 
        {
            this.Attendance = new HashSet<Attendance>();
        }

        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
    }
}