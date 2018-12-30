using System.ComponentModel.DataAnnotations;

namespace r2bw_alpha.Data
{
    public enum PurchaseTypeName 
    {
        Shoes, RaceEntry
    }

    public class PurchaseType
    {
        [Key]
        public string Name { get; set; }
    }
}