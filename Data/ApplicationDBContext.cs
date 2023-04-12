using JobPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<JobModel> Jobs { get; set; }


    }
}
