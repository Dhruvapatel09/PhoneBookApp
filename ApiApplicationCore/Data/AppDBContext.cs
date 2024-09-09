using ApiApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiApplicationCore.Data
{
    public class AppDBContext : DbContext, IAppDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)

        {

        }
        public DbSet<PhoneBookModel> phoneBookModels { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
         public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }
        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>()
             .HasOne(d => d.Country)
             .WithMany(p => p.States)
             .HasForeignKey(d => d.CountryId)
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("FK_State_Country");

           
        }

    }
}
