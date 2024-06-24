using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("Persons")]
public class Person
{
    [Key]
    [Required]
    [Column("IdPerson")]
    public int PersonId { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Column("FirstName")]
    public string PersonFirstName { get; set; }
    
    
    [Required]
    [MaxLength(50)]
    [Column("LastName")]
    public string PersonLastName { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("Address")]
    public string PersonAddress { get; set; }
    
    [Required]
    [EmailAddress]
    [Column("Email")]
    public string PersonEmail { get; set; }
    
    [Required]
    [Phone]
    [Column("Phone")]
    public string PersonPhone { get; set; }
    
    [Required]
    [MaxLength(11)]
    [Column("Pesel")]
    public string PersonPesel { get; set; }
    
    [Column("SoftDelete")]
    public bool PersonSoftDelete { get; set; }
    
    public ICollection<Customer> Customers { get; set; }
}