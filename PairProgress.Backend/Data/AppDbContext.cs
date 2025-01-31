using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PairProgress.Backend.Models;

namespace PairProgress.Backend.Data;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    
}