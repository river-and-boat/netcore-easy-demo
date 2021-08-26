using Microsoft.EntityFrameworkCore;
using UserService.Data.Model;

namespace UserService.Data
{
    public class UserRoleDbContext : DbContext
    {
        public UserRoleDbContext(DbContextOptions<UserRoleDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>()
                .HasKey(manager => new { manager.UserId, manager.RoleId });

            builder.Entity<UserRole>()
                .HasOne(manager => manager.User)
                .WithMany(user => user.Roles)
                .HasForeignKey(manager => manager.UserId);

            builder.Entity<UserRole>()
                .HasOne(manager => manager.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(manager => manager.RoleId);
        }
    }
}
