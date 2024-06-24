using System.ComponentModel.DataAnnotations;

namespace ProjektAPBD.RequestModels.PersonRequestModels;

public class UpdatePersonRequestModel
{
    [Required]
    public int PersonId { get; set; }
    
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
    
}