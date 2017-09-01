using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzAkuten.Data;
using PizzAkuten.Models;

namespace PizzAkuten.Services
{
    public class ManageOrderService
    {
        private readonly ApplicationDbContext _context;
        public ManageOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public Order GetOrderByOrderId(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public List<Order> GetOrdersByApplicationUserId(string applicationUserId)
        {
            return _context.Orders.Where(x => x.ApplicationUserId == applicationUserId).ToList();
        }

        public Order GetOrderByPaymentId(int paymentId)
        {
            return  _context.Orders.FirstOrDefault(x => x.PaymentId == paymentId);
        }

        public List<Order> GetPaymentsByNonAccountUserId(int nonAccountUserId)
        {
            return _context.Orders.Where(x => x.NonAccountUserId == nonAccountUserId).ToList();
        }

        public List<CartItem> GetCartFromOrderId(int orderId)
        {
            var list = _context.Carts.Where(x => x.OrderId == orderId).SelectMany(ci=>ci.CartItems).Include(d=>d.Dish);
            return new List<CartItem>(list);
        }
    }
}
