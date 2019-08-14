using System;

namespace r2bw.Data
{
    public class Attendance
    {
        public Attendance(User user, Meeting meeting)
        {
            this.User = user;
            this.UserId = user.Id;

            this.Meeting = meeting;
            this.MeetingId = meeting.Id;
        }

        public Attendance() { }

        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int MeetingId { get; set; }

        public Meeting Meeting { get; set; }

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
            return (this.UserId == (obj as Attendance).UserId && this.MeetingId == (obj as Attendance).MeetingId);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return Convert.ToInt32(this.UserId) + this.MeetingId;
        }
    }
}