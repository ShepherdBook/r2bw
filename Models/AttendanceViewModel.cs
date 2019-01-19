namespace r2bw_alpha.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using r2bw_alpha.Data;

    public class AttendanceViewModel
    {
        [Required]
        public IEnumerable<int> ParticipantIds { get; set; }
        
        public IEnumerable<Participant> Participants { get; set; }

        [Required]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}