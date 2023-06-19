using Microsoft.EntityFrameworkCore;
using Poke_Api.Models.Pokemon;
using Poke_Api.Models.Rules;
using Poke_Api.Models.User;

namespace Poke_Api.Context
{
    public class DataContext:DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
            
        }

        public DbSet<UserModel> Users { get; set; } 
        public DbSet<RuleModel> Rules { get; set; }
        public DbSet<PokemonModel> Pokemons { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.Rules)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>
                (
                    "UserRule",
                    u => u.HasOne<RuleModel>().WithMany().HasForeignKey("RuleId"),
                    r => r.HasOne<UserModel>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RuleId");

                    }

                );
        }
    }
}
