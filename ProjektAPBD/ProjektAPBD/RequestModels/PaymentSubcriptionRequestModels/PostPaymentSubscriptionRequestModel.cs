using System.ComponentModel.DataAnnotations;

namespace ProjektAPBD.RequestModels.PaymentSubcriptionRequestModels;

public class PostPaymentSubscriptionRequestModel
{
    [Required]
    public int SubscriptionId { get; set; }
    
    [Required]
    public DateTime PaymentDate { get; set; }
    
    [Required]
    public double Amount { get; set; }
}