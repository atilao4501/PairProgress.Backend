using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace PairProgress.Backend.Models;

public class User : IdentityUser
{
    [Required]
    [MaxLength(10)]
    public string UserCode { get; set; }
    
    public List<Goal> Goals { get; set; }
}