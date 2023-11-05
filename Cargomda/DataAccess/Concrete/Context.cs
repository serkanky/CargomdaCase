using Entity.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DEVELOPER\\SQLEXPRESS;initial catalog=CargomdaDb;integrated security=true;");
        }
        

        //Veritabanı tablosu sınıfları.
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        
    }
}

//Bağlantı dizesini Context sınıfında tanımladım. appsettings.json dosyasında bağlantı dizesini tanımlamaya gerek duymadım çünkü projenin basitliği bu yaklaşımı destekliyor 

