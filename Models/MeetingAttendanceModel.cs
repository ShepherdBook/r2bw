namespace r2bw.Models
{
    using System.Collections.Generic;
    using r2bw.Data;

    public class MeetingAttendanceModel
    {
        public MeetingAttendanceModel()
        {
            AllParticipants = new List<User>();
            Present = new List<User>();
        }

        public Meeting Meeting { get; set; }

        public List<User> AllParticipants { get; set; }

        public List<User> Present { get; set; }

        public int PresentCount { get { return Present.Count; } }

        public string[] Attendees { get; set; }
    }
}