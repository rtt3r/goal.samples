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

        public DbSet<Application> Applications { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Identity");

            modelBuilder.Entity<IdentityUserClaim<string>>(builder => { builder.ToTable("UserClaims"); });
            modelBuilder.Entity<IdentityUserLogin<string>>(builder => { builder.ToTable("UserLogins"); });
            modelBuilder.Entity<IdentityUserToken<string>>(builder => { builder.ToTable("UserTokens"); });
            modelBuilder.Entity<IdentityRoleClaim<string>>(builder => { builder.ToTable("RoleClaims"); });
            modelBuilder.Entity<IdentityUserRole<string>>(builder => { builder.ToTable("UserRoles"); });
            modelBuilder.Entity<Role>(builder =>
            {
                builder.ToTable("Roles");

                builder.Property(p => p.Description)
                .HasMaxLength(512);

                builder.Property(p => p.ApplicationId)
                    .HasMaxLength(64);

                builder.HasIndex(p => new { p.ApplicationId, p.NormalizedName })
                    .IsUnique()
                    .HasFilter($"[{nameof(Role.NormalizedName)}] IS NOT NULL");

                builder.HasMany(e => e.RoleMembers)
                    .WithMany(e => e.MemberOf)
                    .UsingEntity(
                        "RoleRoles",
                        l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                        r => r.HasOne(typeof(Role)).WithMany().HasForeignKey("MemberId").HasPrincipalKey(nameof(Role.Id)),
                        j => j.HasKey("RoleId", "MemberId"));

                builder.HasMany(p => p.Authorizations)
                    .WithOne(p => p.Role)
                    .HasForeignKey(p => p.RoleId);
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users");

                builder.Property(p => p.Name)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.Property(p => p.NormalizedName)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.HasIndex(p => p.NormalizedName)
                    .HasDatabaseName("NameIndex")
                    .IsUnique()
                    .HasFilter($"[{nameof(User.NormalizedName)}] IS NOT NULL");

                builder.HasMany(p => p.Authorizations)
                    .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId);
            });

            modelBuilder.Entity<Application>(builder =>
            {
                builder.ToTable("Applications", "Identity");
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Id)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(p => p.Name)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.Property(p => p.NormalizedName)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.Property(p => p.Description)
                    .HasMaxLength(512);

                builder.HasIndex(p => p.NormalizedName)
                    .IsUnique();

                builder.HasMany(p => p.Roles)
                    .WithOne(p => p.Application)
                    .HasForeignKey(p => p.ApplicationId);

                builder.HasMany(p => p.Resources)
                    .WithOne(p => p.Application)
                    .HasForeignKey(p => p.ApplicationId);

                builder.HasMany(p => p.Operations)
                    .WithOne(p => p.Application)
                    .HasForeignKey(p => p.ApplicationId);
            });

            modelBuilder.Entity<Authorization>(builder =>
            {
                builder.ToTable("Authorizations", "Identity");
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Id)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(p => p.PermissionId)
                    .HasMaxLength(64);

                builder.Property(p => p.UserId)
                    .HasMaxLength(450);

                builder.Property(p => p.RoleId)
                    .HasMaxLength(450);

                builder.HasIndex(p => new { p.PermissionId, p.UserId })
                    .IsUnique()
                    .HasFilter($"[{nameof(Authorization.UserId)}] IS NOT NULL");

                builder.HasIndex(p => new { p.PermissionId, p.RoleId })
                    .IsUnique()
                    .HasFilter($"[{nameof(Authorization.RoleId)}] IS NOT NULL");
            });

            modelBuilder.Entity<Operation>(builder =>
            {
                builder.ToTable("Operations", "Identity");
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Id)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(p => p.Name)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.Property(p => p.NormalizedName)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.Property(p => p.Description)
                    .HasMaxLength(512);

                builder.Property(p => p.ApplicationId)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.HasIndex(p => new { p.ApplicationId, p.NormalizedName })
                    .IsUnique();

                builder.HasMany(p => p.Permissions)
                    .WithOne(p => p.Operation)
                    .HasForeignKey(p => p.OperationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Permission>(builder =>
            {
                builder.ToTable("Permissions", "Identity");
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Id)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(p => p.OperationId)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(p => p.ResourceId)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.HasIndex(p => new { p.ResourceId, p.OperationId })
                    .IsUnique();
            });

            modelBuilder.Entity<Resource>(builder =>
            {
                builder.ToTable("Resources", "Identity");
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Id)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(p => p.Name)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.Property(p => p.NormalizedName)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.Property(p => p.Description)
                    .HasMaxLength(512);

                builder.Property(p => p.ApplicationId)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.HasIndex(p => new { p.ApplicationId, p.NormalizedName })
                    .IsUnique();

                builder.HasMany(p => p.Permissions)
                    .WithOne(p => p.Resource)
                    .HasForeignKey(p => p.ResourceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}