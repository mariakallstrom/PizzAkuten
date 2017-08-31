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
            return _context.Users.Find(applicationuserId);
        }

        public NonAccountUser GetNonAccountUserById(int nonAccountUserId)
        {
            return _context.NonAccountUsers.Find(nonAccountUserId);
        }
    }
}
