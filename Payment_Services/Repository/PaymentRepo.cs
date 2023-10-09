using ManhNhung_Payment_Services.DataContext;
using Payment_Services.Interface;

namespace ManhNhung_Payment_Services.Repository
{
    public class PaymentRepo : IPayment
    {
        private readonly PaymentDbContext _context;
        public PaymentRepo(PaymentDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNewPayement()
        {
            
            return true;
        }
    }
}
