using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Models;
using System;

namespace PhoneBookApp.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)

        {

        }
        public DbSet<PhoneBookModel> phoneBookModels { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
