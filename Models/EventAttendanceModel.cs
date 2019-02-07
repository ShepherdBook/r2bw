namespace r2bw.Models
{
    using System.Collections.Generic;
    using r2bw.Data;

    public class EventAttendanceModel
    {
        public Event Event { get; set; }

        public List<Participant> AllParticipants { get; set; }

        public int[] Attendees { get; set; }
    }
}