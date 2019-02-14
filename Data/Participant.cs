namespace r2bw.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Participant
    {

        public Participant()
        {
            this.Attendance = new HashSet<Attendance>();
            this.Purchases = new HashSet<Purchase>();
        }

        [Required]
        public int Id { get; set; }

        public string Name { get { return $"{FirstName} {LastName}"; } }

        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Waiver signed on")]
        [DataType(DataType.Date)]
        public DateTimeOffset? WaiverSignedOn { get; set; }

        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string Sex { get; set; }

        public string Size { get; set; }

        [Required]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        public ICollection<Attendance> Attendance { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}