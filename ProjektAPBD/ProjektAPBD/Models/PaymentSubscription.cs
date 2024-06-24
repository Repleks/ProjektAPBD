using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("PaymentsSubscriptions")]
public class PaymentSubscription
{
    [Key]
    [Required]
    [Column("IdPayment")]
    public int PaymentId { get; set; }
    
    [Required]
    [ForeignKey("Subscription")]
    [Column("IdSubscription")]
    public int SubscriptionId { get; set; }
    
    public Subscription Subscription { get; set; }
    
    [Required]
    [Column("PaymentDate")]
    public DateTime PaymentDate { get; set; }
    
    [Required]
    [Column("Amount")]
    public double Amount { get; set; }
}