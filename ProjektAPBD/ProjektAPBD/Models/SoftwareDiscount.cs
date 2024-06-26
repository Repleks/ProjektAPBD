using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("SoftwareDiscounts")]
public class SoftwareDiscount
{
    [Key]
    [Required]
    [Column("IdSoftwareDiscount")]
    public int SoftwareDiscountId { get; set; }
    
    [Required]
    [ForeignKey("Software")]
    [Column("IdSoftware")]
    public int SoftwareId { get; set; }
    
    public Software Software { get; set; }
    
    [Required]
    [ForeignKey("Discount")]
    [Column("IdDiscount")]
    public int DiscountId { get; set; }
    
    public Discount Discount { get; set; }
    
}