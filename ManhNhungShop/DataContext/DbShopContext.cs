using ManhNhungShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ManhNhungShop.DataContext
{
    public class DbShopContext : DbContext
    {
        public DbShopContext(DbContextOptions<DbShopContext> options) : base(options)
        {

        }
        //PRODUCT
        public DbSet<Products> Products { get; set; }
        //poduct delteSoft
        public DbSet<ProductDeleSofts> ProductDeleSofts { set; get ;}
        // PRRODUCT DETAIL
        public DbSet<ProductsDetails> ProductsDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}
