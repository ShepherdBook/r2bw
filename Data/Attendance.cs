namespace r2bw_alpha.Data
{
    public class Attendance
    {
        public int Id { get; set; }

        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}