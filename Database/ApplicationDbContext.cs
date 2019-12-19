using EsxiRestfulApi.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EsxiRestfulApi.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<VSwitch> VSwitches { get; set; }
        public DbSet<PortGroup> PortGroups { get; set; }
        public DbSet<Filesystem> Filesystems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // ...
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VSwitch>(entity =>
            {
                entity.HasMany(vs => vs.PortGroups)
                    .WithOne(pg => pg.VSwitch)
                    .HasForeignKey(pg => pg.VSwitchId);
            });
        }
    }
}