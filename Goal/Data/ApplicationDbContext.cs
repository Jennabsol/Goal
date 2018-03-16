using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Goal.Models;

namespace Goal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Goals> Goals { get; set; }
        public DbSet<DailySprints> DailySprints { get; set; }
        public DbSet<GoalSprintGroup> GoalSprintGroup { get; set; }
        public DbSet<Retrospective> Retrospective { get; set; }
        public DbSet<SprintGroup> SprintGroup { get; set; }
        




        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Goals>()
            //  .Property(b => b.DateCreated)
            //  .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            //builder.Entity<DailySprints>()
            //  .Property(b => b.DateCreated)
            //  .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            //builder.Entity<Retrospective>()
            //  .Property(b => b.DateCreated)
            //  .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            //builder.Entity<SprintGroup>()
            //  .Property(b => b.DateCreated)
            //  .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
