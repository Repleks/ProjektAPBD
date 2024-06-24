using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("PaymentsContracts")]
public class PaymentContract
{
    [Key]
    [Required]
    [Column("IdPayment")]
    public int PaymentId { get; set; }
    
    [Required]
    [ForeignKey("Contract")]
    [Column("IdContract")]
    public int ContractId { get; set; }
    
    public Contract Contract { get; set; }
    
    [Required]
    [Column("PaymentDate")]
    public DateTime PaymentDate { get; set; }
    
    [Required]
    [Column("Amount")]
    public double Amount { get; set; }
}