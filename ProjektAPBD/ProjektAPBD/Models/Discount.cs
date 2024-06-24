using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("Discounts")]
public class Discount
{
    [Key]
    [Required]
    [Column("IdDiscount")]
    public int DiscountId { get; set; }
    
    [Required]
    [Column("Description")]
    public string DiscountDescription { get; set; }
    
    
    [Required]
    [Column("Value")]
    public int DiscountValue { get; set; }
    
    [Required]
    [Column("DateStart")]
    public DateTime DiscountDateStart { get; set; }
    
    [Required]
    [Column("DateEnd")]
    public DateTime DiscountDateEnd { get; set; }
    
    public ICollection<ContractDiscount> ContractDiscounts { get; set; }
    
    public ICollection<SubscriptionDicount> SubscriptionDicounts { get; set; }
}