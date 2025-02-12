using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PairProgress.Backend.Models;

public class UserDuo
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string User1Code { get; set; }
    
    [ForeignKey("User1Code")]
    public User User1 { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string User2Code { get; set; }
    
    [ForeignKey("User2Code")]
    public User User2 { get; set; }
}