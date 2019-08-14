using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace r2bw.Data
{
    public class Group
    {
        public Group()
        {
            this.Users = new HashSet<User>();
            this.Meetings = new HashSet<Meeting>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("Group")]
        public string Name { get; set; }

        public bool Active { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Meeting> Meetings { get; set; }
    }
}