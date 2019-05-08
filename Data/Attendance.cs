namespace r2bw.Data
{
    public class Attendance
    {
        public Attendance(Participant p, Event e)
        {
            this.Participant = p;
            this.ParticipantId = p.Id;

            this.Event = e;
            this.EventId = e.Id;
        }

        public Attendance() { }

        public int Id { get; set; }

        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public bool Active { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            // TODO: write your implementation of Equals() here
            return (this.ParticipantId == (obj as Attendance).ParticipantId && this.EventId == (obj as Attendance).EventId);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return this.ParticipantId + this.EventId;
        }
    }
}