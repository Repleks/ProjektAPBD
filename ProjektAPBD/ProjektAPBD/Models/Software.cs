using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("Software")]
public class Software
{
    [Key]
    [Required]
    [Column("IdSoftware")]
    public int SoftwareId { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("Name")]
    public string SoftwareName { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("Description")]
    public string SoftwareDescription { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("CurrentVersion")]
    public string SoftwareCurrentVersion { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("Category")]
    public string SoftwareCategory { get; set; }
    
    public ICollection<Contract> Contracts { get; set; }
    
    public ICollection<SoftwareVersion> SoftwareVersions { get; set; }
}