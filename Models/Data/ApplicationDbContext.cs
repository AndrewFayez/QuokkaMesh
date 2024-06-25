

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuokkaMesh.Models.DataModel.OTPModel;
using QuokkaMesh.Models.DataModels.CartCategory.Cart;
using QuokkaMesh.Models.DataModels.CartCategory.Category;

namespace QuokkaMesh.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }
        public DbSet<OTPEmailModel> OTPEmail { get; set; }
        public DbSet<CartModel> Cart { get; set; }

        public DbSet<CategoryModel> Categories { get; set; }

        public DbSet<CategoryCart> CategoryCart { get; set; }
        public DbSet<UserCart> UserCart { get; set; }
        public DbSet<CartAndUserCart> CartAndUserCart { get; set; }










        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
              .HasKey(u => new {
                  u.Id
              });



            builder.Entity<CartModel>()
              .HasKey(p => new {
                  p.Id
              });

            builder.Entity<CartAndUserCart>()
              .HasKey(up => new
              {
                  up.UserCartId,
                  up.CartId
              });

            builder.Entity<CartAndUserCart>()
              .HasOne(up => up.UserCart)
              .WithMany(u => u.CartAndUserCart)
              .HasForeignKey(u => u.UserCartId)
               .OnDelete(DeleteBehavior.NoAction); ;

            builder.Entity<CartAndUserCart>()
              .HasOne(up => up.Cart)
              .WithMany(p => p.CartAndUserCart)
              .HasForeignKey(p => p.CartId)
              .OnDelete(DeleteBehavior.NoAction); ;

            ////////////////////////////////////////////////////////////////////////


            builder.Entity<CategoryModel>()
              .HasKey(p => new {
                  p.Id
              });

            builder.Entity<CategoryCart>()
              .HasKey(up => new {
                  up.CategoryId,
                  up.CartId
              });

            builder.Entity<CategoryCart>()
              .HasOne(up => up.Category)
              .WithMany(u => u.CategoryCart)
              .HasForeignKey(u => u.CategoryId)
               .OnDelete(DeleteBehavior.NoAction); ;

            builder.Entity<CategoryCart>()
              .HasOne(up => up.Cart)
              .WithMany(p => p.CategoryCart)
              .HasForeignKey(p => p.CartId)
              .OnDelete(DeleteBehavior.NoAction); ;



        }
    }
}
