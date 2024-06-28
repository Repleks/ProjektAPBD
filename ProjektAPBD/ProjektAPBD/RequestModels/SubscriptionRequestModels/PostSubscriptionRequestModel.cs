using System.ComponentModel.DataAnnotations;

namespace ProjektAPBD.RequestModels.SubscriptionRequestModels;

public class PostSubscriptionRequestModel
{
    [Required]
    public int CustomerId { get; set; }
    
    [Required]
    public int SoftwareId { get; set; }
    
    [Required]
    public string SubscriptionName { get; set; }
    
    [Required]
    public DateTime RenewalDate { get; set; }
    
    [Required]
    public double PricePerMonth { get; set; }
    
}