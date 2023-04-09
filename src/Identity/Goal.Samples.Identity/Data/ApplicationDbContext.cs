using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Goal.Samples.Identity.Models;

namespace Goal.Samples.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Identity");

            builder.Entity<IdentityUserClaim<string>>(b => { b.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(b => { b.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(b => { b.ToTable("UserTokens"); });
            builder.Entity<IdentityRoleClaim<string>>(b => { b.ToTable("RoleClaims"); });
            builder.Entity<IdentityUserRole<string>>(b => { b.ToTable("UserRoles"); });
            builder.Entity<Role>(b => { b.ToTable("Roles"); });

            builder.Entity<User>(b =>
            {
                b.ToTable("Users");

                b.Property(p => p.Name)
                    .HasMaxLength(256)
                    .IsRequired();

                b.Property(p => p.NormalizedName)
                    .HasMaxLength(256)
                    .IsRequired();
            });
        }
    }
}