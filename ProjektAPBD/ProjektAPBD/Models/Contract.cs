using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("Contracts")]
public class Contract
{
    [Key]
    [Required]
    [Column("IdContract")]
    public int IdContract { get; set; }
    
    [ForeignKey("Customer")]
    [Column("IdCustomer")]
    public int IdCustomer { get; set; }
    
    public Customer Customer { get; set; }
    
    [ForeignKey("Software")]
    [Column("IdSoftware")]
    public int IdSoftware { get; set; }
    
    public Software Software { get; set; }
    
    [Required]
    [Column("DateFrom")]
    public DateTime DateFrom { get; set; }
    
    [Required]
    [Column("DateTo")]
    public DateTime DateTo { get; set; }
    
    [Required]
    [Column("PricePerMonth")]
    public double PricePerMonth { get; set; }

    [Column("AdditionalSupport")]
    public int AdditionalSupport { get; set; } = 0;
    
    [Required]
    [Column("TotalPrice")]
    public double TotalPrice { get; set; }

    [Required]
    [Column("Signed")]
    public bool Signed { get; set; } = false;
    
    public ICollection<ContractDiscount> ContractDiscounts { get; set; }
}