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

        public  Payment CreatePayment(IFormCollection form)
        {
            var paymentChoize = Convert.ToInt32(form["creditCardRadio"]);

            var model = new Payment();

            if (String.IsNullOrEmpty(form["NonAccountUserId"]))
            {
                model.ApplicationUserId = form["UserId"];
            }
            else
            {
                model.NonAccountUserId = Convert.ToInt32(form["NonAccountUserId"]);
            }
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

            model.OrderId = Convert.ToInt32(form["OrderId"]);

            _context.Payments.Add(model);
            _context.SaveChanges();
            return model;
        }
    }
}
