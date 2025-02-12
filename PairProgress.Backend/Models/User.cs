using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PairProgress.Backend.Models;

public class User : IdentityUser
{
    [Required]
    [MaxLength(10)]
    public string UserCode { get; set; }
}