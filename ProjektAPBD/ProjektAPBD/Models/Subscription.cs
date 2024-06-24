using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("Subscriptions")]
public class Subscription
{
    [Key]
    [Required]
    [Column("IdSubscription")]
    public int SubscriptionId { get; set; }
    
    [Required]
    [Column("Customer")]
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    
    public Customer Customer { get; set; }
    
    [Required]
    [Column("Software")]
    [ForeignKey("Software")]
    public int SoftwareId { get; set; }
    
    public Software Software { get; set; }
    
    [Required]
    [Column("RenewalDate")]
    public DateTime RenewalDate { get; set; }
    
    [Required]
    [Column("PricePerMonth")]
    public double PricePerMonth { get; set; }
    
    [Column("ActiveStatus")]
    public bool ActiveStatus { get; set; } = true;
    
    public ICollection<SubscriptionDicount> SubscriptionDicounts { get; set; }
    
    public ICollection<PaymentSubscription> PaymentSubscriptions { get; set; }
}