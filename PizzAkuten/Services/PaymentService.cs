using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzAkuten.Data;
using PizzAkuten.Models;

namespace PizzAkuten.Services
{
    public class PaymentService
    {
        private readonly ApplicationDbContext _context;
        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Payment> GetAllPayments()
        {
            return _context.Payments.ToList();
        }

        public Payment GetPaymentByPaymentId(int paymentId)
        {
            return _context.Payments.FirstOrDefault(x => x.PaymentId == paymentId);
        }

        public Payment GetPaymentByOrderId(int orderId)
        {
            return _context.Payments.FirstOrDefault(x => x.OrderId == orderId);
        }

        public List<Payment> GetPaymentsByApplicationUserId(string applicationUserId)
        {
            return _context.Payments.Where(x => x.ApplicationUserId == applicationUserId).ToList();
        }

        public List<Payment> GetPaymentsByNonAccountUserId(int nonAccountUserId)
        {
            return _context.Payments.Where(x => x.NonAccountUserId == nonAccountUserId).ToList();
        }
    }
}
