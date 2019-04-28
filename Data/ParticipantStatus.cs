namespace r2bw.Data
{
    public enum ParticipantStatusValue
    {
        Pending = 1, Active = 2, Inactive = 3
    }

    public class ParticipantStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}