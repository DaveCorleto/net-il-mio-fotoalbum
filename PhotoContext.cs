using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;
using System.Drawing;
using net_il_mio_fotoalbum.Data;

namespace net_il_mio_fotoalbum
{
    public class PhotoContext : IdentityDbContext<IdentityUser>
    {

        //Riferimenti alle tabelle
        public DbSet<Photo> Photos { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Email> Emails { get; set; }
        public DbSet<Message> Messages { get; set; }
        public object Message { get; internal set; }

        public const string _connectionString = "Data Source=localhost;Initial Catalog=db-photos;Integrated Security = True;Pooling=False;Encrypt=True;TrustServerCertificate=True";
        public PhotoContext()
        {
        }
        public PhotoContext(DbContextOptions<PhotoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
