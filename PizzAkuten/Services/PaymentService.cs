using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            var paymentid = _context.Orders.Find(orderId).PaymentId;
            return _context.Payments.FirstOrDefault(x => x.PaymentId == paymentid);
        }

        public List<Payment> GetPaymentsByApplicationUserId(string applicationUserId)
        {
            var paymentIds = _context.Orders.Where(x=>x.ApplicationUserId == applicationUserId).Select(p=>p.PaymentId);
            var list = new List<Payment>();

            foreach (var item in paymentIds)
            {
                list.Add(_context.Payments.Find(item));
            }
            return list;
        }

        public List<Payment> GetPaymentsByNonAccountUserId(int nonAccountUserId)
        {
            var paymentIds = _context.Orders.Where(x => x.NonAccountUserId == nonAccountUserId).Select(p => p.PaymentId);
            var list = new List<Payment>();

            foreach (var item in paymentIds)
            {
                list.Add(_context.Payments.Find(item));
            }
            return list;
        }

        public  Payment CreatePayment(IFormCollection form)
        {
            var paymentChoize = Convert.ToInt32(form["creditCardRadio"]);
            var orderId = Convert.ToInt32(form["OrderId"]);
            var order = _context.Orders.FirstOrDefault(x=>x.OrderId == orderId);
            var model = new Payment();

            if (paymentChoize != 3)
            {
                model.Month = Convert.ToInt32(form["Month"]);
                model.CardNumber = form["CardNumber"];
                model.Cvv = Convert.ToInt32(form["Cvv"]);
                model.Year = Convert.ToInt32(form["Year"]);
            }

            if (paymentChoize == 1)
            {
                model.PayMethod = "Visa";
            }
            else if (paymentChoize == 2)
            {
                model.PayMethod = "MasterCard";
            }
            else
            {
                model.PayMethod = "Swish";
            }

            _context.Payments.Add(model);
            _context.SaveChanges();

            return SavePayment(order);
        }

     public Payment SavePayment(Order order)
        {
            var lastPayment = _context.Payments.Last();
            order.PaymentId = lastPayment.PaymentId;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return lastPayment;
        }
    }
}
