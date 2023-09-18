using ManhNhungShop_Account_Services.Models;
using Microsoft.EntityFrameworkCore;

namespace ManhNhungShop_Account_Services.DataContext
{
    public class DataAccountContext : DbContext
    {
        public DataAccountContext(DbContextOptions<DataAccountContext> options) : base(options) {

        }

        public DbSet<Accounts> Accounts { set; get; }

        public DbSet<AccountsDetails> AccountsDetails { set; get; }

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
