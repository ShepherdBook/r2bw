namespace R2bw.Core.Entities
{
    using System.ComponentModel.DataAnnotations;

    public enum PurchaseTypeName 
    {
        Apparel, RaceEntry, Shoes
    }

    public class PurchaseType
    {
        [Key]
        public string Name { get; set; }
    }
}