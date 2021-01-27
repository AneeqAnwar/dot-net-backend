using System;
using Books_Inventory_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Books_Inventory_System.Data
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();

        DbSet<Book> Books { get; set; }
        DbSet<User> Users { get; set; }
    }
}
