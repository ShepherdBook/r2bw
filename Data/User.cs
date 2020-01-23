namespace r2bw.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.Attendance = new HashSet<Attendance>();
            this.Purchases = new HashSet<Purchase>();
        }

        public string Name { get { return $"{FirstName} {LastName}"; } }

        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Waiver signed on")]
        [DataType(DataType.Date)]
        public DateTimeOffset WaiverSignedOn { get; set; }

        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string Sex { get; set; }

        public string Size { get; set; }

        [DisplayName("Shoe Size")]
        public string ShoeSize { get; set; }

        [DisplayName("Street")]

        public string Street1 { get; set; }

        [DisplayName("")]

        public string Street2 { get; set; }

        public string City { get; set; }

        [StringLength(2, ErrorMessage = "State must be two letters")]
        public string State { get; set; }

        [StringLength(5)]
        public string Zip { get; set; }

        [DisplayName("Dues Paid on")]
        [DataType(DataType.Date)]
        public DateTimeOffset? DuesPaidOn { get; set; }

        public bool Active { get; set; }

        public int? GroupId { get; set; }

        public Group Group { get; set; }

        public ICollection<Attendance> Attendance { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        [DisplayName("Full Name")]
        public string FullName {
            get {
                return $"{FirstName} {LastName}";
            }
        }

        public string EmailConfirmedText {
            get {
                return this.EmailConfirmed ? "Yes" : "No";
            }
        }
    }
}