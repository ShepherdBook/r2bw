namespace r2bw_alpha.Data
{
    using System.Collections.Generic;

    public class Participant
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
    }
}