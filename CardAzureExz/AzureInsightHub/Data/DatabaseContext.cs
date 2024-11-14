using AzureInsightHub.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureInsightHub.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Card> Cards { get; set; }
    }
}
