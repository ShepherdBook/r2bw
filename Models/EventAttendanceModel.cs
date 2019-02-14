namespace r2bw.Models
{
    using System.Collections.Generic;
    using r2bw.Data;

    public class EventAttendanceModel
    {
        public Event Event { get; set; }

        public List<Participant> AllParticipants { get; set; }

        public List<Participant> Present { get; set; }

        public int PresentCount { get { return Present.Count; } }

        public int[] Attendees { get; set; }
    }
}