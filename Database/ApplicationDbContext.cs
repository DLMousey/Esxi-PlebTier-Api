using Microsoft.EntityFrameworkCore;

namespace EsxiRestfulApi.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // ...
        }
    }
}