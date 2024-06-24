using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjektAPBD.Models;

[Table("Contracts_Discounts")]
[PrimaryKey("IdContract", "IdDiscount")]
public class ContractDiscount
{
    [Key]
    [Required]
    [ForeignKey("Contract")]
    [Column("IdContract")]
    public int IdContract { get; set; }
    
    public Contract Contract { get; set; }
    
    [Key]
    [Required]
    [ForeignKey("Discount")]
    [Column("IdDiscount")]
    public int IdDiscount { get; set; }
    
    public Discount Discount { get; set; }
}