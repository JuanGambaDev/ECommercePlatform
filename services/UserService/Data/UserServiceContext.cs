using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;

public class UserServiceContext : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public UserServiceContext(DbContextOptions<UserServiceContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<User>
        (
            User =>
            {
                User.ToTable("user_ecommerce");
                User.HasKey(u => u.UserId);
                User.Property(u => u.UserId).ValueGeneratedOnAdd();
                User.Property(u => u.Name).IsRequired().HasMaxLength(100);
                User.Property(u => u.Email).IsRequired().HasMaxLength(256);
                User.HasIndex(e => e.Email).IsUnique();
                User.Property(u => u.PasswordHash).IsRequired().HasMaxLength(200);
            }
        );

        modelBuilder.Entity<RefreshToken>
        (
            RefreshToken =>
            {
                RefreshToken.ToTable("refresh_tokens");
                RefreshToken.HasKey(t => t.TokenId);
                RefreshToken.Property(t => t.TokenId).ValueGeneratedOnAdd();
                RefreshToken.Property(t => t.Token).IsRequired().HasMaxLength(600);
                RefreshToken.Property(t => t.ExpiryDate).IsRequired();
                RefreshToken.Property(t => t.Revoked).IsRequired().HasDefaultValue(false);
                RefreshToken.Property(t => t.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                RefreshToken.Property(t => t.UserId).IsRequired();
                RefreshToken.HasOne(t => t.User).WithMany(u => u.RefreshTokens).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
            }
        );
    }
}