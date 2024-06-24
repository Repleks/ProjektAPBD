using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjektAPBD.Models;

[Table("Subscriptions_Discounts")]
[PrimaryKey("IdSubscription", "IdDiscount")]
public class SubscriptionDicount
{
    [ForeignKey("Subscription")]
    public int IdSubscription { get; set; }
    
    public Subscription Subscription { get; set; }
    
    [ForeignKey("Discount")]
    public int IdDiscount { get; set; }
    
    public Discount Discount { get; set; }
}