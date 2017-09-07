using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using PizzAkuten.Models;

namespace XUnitTestPizzAkuten.Fakes
{
    class FakePasswordHasher : IPasswordHasher<ApplicationUser>
    {

        public string HashPassword(ApplicationUser user, string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
