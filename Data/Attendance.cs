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

            this.Active = true;
        }

        public Attendance() { }

        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int MeetingId { get; set; }

        public Meeting Meeting { get; set; }

        public bool Active { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return (this.UserId == (obj as Attendance).UserId 
                && this.MeetingId == (obj as Attendance).MeetingId);
        }
        
        public override int GetHashCode()
        {
            return this.MeetingId + this.UserId.GetHashCode();
        }
    }
}