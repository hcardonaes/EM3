using Microsoft.EntityFrameworkCore;

namespace EM3.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Personaje> Personajes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Millennium.db");
        }
    }

    public class Personaje
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Nivel { get; set; }
    }
}
