using Books_Inventory_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Books_Inventory_System.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
