using System.Collections.Generic;

namespace r2bw_alpha.Data
{
    public class Group
    {
        public Group()
        {
            this.Participants = new HashSet<Participant>();
            this.Events = new HashSet<Event>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Participant> Participants { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}