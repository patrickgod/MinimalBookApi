global using Microsoft.EntityFrameworkCore;

namespace MinimalBookApi
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=minimalbookdb;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        public DbSet<Book> Books => Set<Book>();
    }
}
