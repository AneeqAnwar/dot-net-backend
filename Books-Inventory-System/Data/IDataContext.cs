using System;
using System.Threading.Tasks;
using Books_Inventory_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Books_Inventory_System.Data
{
    public interface IDataContext : IDisposable
    {
        Task<int> SaveChangesAsync();
        int SaveChanges();

        DbSet<Book> Books { get; set; }
        DbSet<User> Users { get; set; }
    }
}
