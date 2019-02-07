using System.ComponentModel.DataAnnotations;

namespace r2bw.Data
{
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