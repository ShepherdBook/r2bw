namespace r2bw_alpha.Data
{
    using System;
    using System.Collections.Generic;

    public class Participant
    {

        public Participant()
        {
            this.Attendance = new HashSet<Attendance>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? WaiverSignedOn { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
    }
}