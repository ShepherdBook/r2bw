namespace r2bw_alpha.Data
{
    using System;
    using Microsoft.AspNetCore.Identity;

    public class Purchase
    {
        public int Id { get; set; }

        public PurchaseType Type { get; set; }

        public DateTime PurchasedOn { get; set; }

        public double Amount { get; set; }

        public int ParticipantId { get; set; }

        public Participant Participant { get; set; }
    }
}