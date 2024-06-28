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
    [Column("Name")]
    public string SubscriptionName { get; set; }
    
    [Required]
    [Column("RenewalDate")]
    public DateTime RenewalDate { get; set; }
    
    [Required]
    [Column("PricePerMonth")]
    public double PricePerMonth { get; set; }
    
    [Required]
    [Column("FirstPaymentPrice")]
    public double FirstPaymentPrice { get; set; }

    [Column("ActiveStatus")]
    public bool ActiveStatus { get; set; } = true;

    [Column("ActivationDate")]
    public DateTime ActivationDate { get; set; }

    public ICollection<PaymentSubscription> PaymentSubscriptions { get; set; }
}