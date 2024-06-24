using System.ComponentModel.DataAnnotations;

namespace ProjektAPBD.RequestModels.PersonRequestModels;

public class PostPersonRequestModel
{
    [Required]
    [MaxLength(50)]
    public string PersonFirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string PersonLastName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string PersonAddress { get; set; }
    
    [Required]
    [EmailAddress]
    public string PersonEmail { get; set; }
    
    [Required]
    [Phone]
    public string PersonPhone { get; set; }
    
    [Required]
    [MaxLength(11)]
    public string PersonPesel { get; set; }
    
}