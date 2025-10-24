using ClinicBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            builder.Entity<IdentityRole<long>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<long>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<long>>().ToTable("UserTokens");
        }
    }
}