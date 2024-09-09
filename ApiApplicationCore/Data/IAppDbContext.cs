using ApiApplicationCore.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApiApplicationCore.Data
{
    public interface IAppDbContext: IDbContext
    {
        public DbSet<PhoneBookModel> phoneBookModels { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
    }
}
