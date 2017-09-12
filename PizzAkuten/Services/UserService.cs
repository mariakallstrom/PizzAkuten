using Microsoft.AspNetCore.Identity;
using PizzAkuten.Data;
using PizzAkuten.Models;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PizzAkuten.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;


        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public ApplicationUser GetApplicationUser()
        {
            var User =  _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var aUser = _context.Users.Find(User);

            if(aUser != null)
            {
                return aUser;
            }
            return null;
        }

  

        public ApplicationUser GetApplicationUserById(string applicationuserId)
        {
            return _context.Users.FirstOrDefault(x=>x.Id == applicationuserId);
        }

        public NonAccountUser GetNonAccountUserById(int? nonAccountUserId)
        {
            return _context.NonAccountUsers.FirstOrDefault(x=>x.NonAccountUserId == nonAccountUserId);
        }

        public List<NonAccountUser> GetAllNonAccountUsers()
        {
            return _context.NonAccountUsers.ToList();
        }

        public string GetApplicationUserEmailByOrderId(int? orderId)
        {
            var userId = _context.Orders.FirstOrDefault(x => x.OrderId == orderId).ApplicationUserId;
            if (userId == null)
            {
                return "NonAccountUser";
            }
            return _context.Users.Find(userId).Email;
        }

        public string GetNonAccountUserEmailByOrderId(int? orderId)
        {
            var userId = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            if (userId.NonAccountUserId == null)
            {
                return "AccountUser";
            }
            return _context.NonAccountUsers.Find(userId).Email;

        }

        public string GetApplicationUserRole(String aUserid)
        {
            var roleId = _context.UserRoles.FirstOrDefault(x => x.UserId == aUserid).RoleId;

            return _context.Roles.Find(roleId).Name;

        }
    }
}
