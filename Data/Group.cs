using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace r2bw.Data
{
    public class Group
    {
        public Group()
        {
            this.Participants = new HashSet<Participant>();
            this.Events = new HashSet<Event>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("Group")]
        public string Name { get; set; }

        public bool Active { get; set; }

        public ICollection<Participant> Participants { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}