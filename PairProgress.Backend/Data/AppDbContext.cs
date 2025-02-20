using System.Collections.Immutable;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PairProgress.Backend.Models;

namespace PairProgress.Backend.Data;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<UserDuo> UserDuos { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Contribution> Contributions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserCode)
            .IsUnique();

        modelBuilder.Entity<UserDuo>()
            .HasOne(duo => duo.User1)
            .WithOne()
            .HasForeignKey<UserDuo>(duo => duo.User1Code)
            .HasPrincipalKey<User>(u => u.UserCode)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserDuo>()
            .HasOne(duo => duo.User2)
            .WithOne()
            .HasForeignKey<UserDuo>(duo => duo.User2Code)
            .HasPrincipalKey<User>(u => u.UserCode)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserDuo>()
            .HasIndex(duo => duo.User1Code )
            .IsUnique();

        modelBuilder.Entity<UserDuo>()
            .HasIndex(duo => duo.User2Code)
            .IsUnique();
        
        modelBuilder.Entity<Goal>()
            .HasOne(g => g.User)
            .WithMany(u => u.Goals)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Goal>()
            .HasMany(g => g.Contributions)
            .WithOne(c => c.Goal)
            .OnDelete(DeleteBehavior.Cascade);
        
        
    }
}