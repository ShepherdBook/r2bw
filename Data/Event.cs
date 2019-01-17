namespace r2bw_alpha.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Event
    {

        public Event() 
        {
            this.Attendance = new HashSet<Attendance>();
        }

        [Required]
        public int Id { get; set; }
        
        public string Name { get; set; }

        [Required]
        [DisplayName("Date and Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:f}")]
        public DateTime Timestamp { get; set; }

        [Required]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        public string DisplayName { get { return $"{Timestamp.ToString("g")} - {Group.Name} - {Name}"; } }

        public ICollection<Attendance> Attendance { get; set; }
    }
}