using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace XUnitTestPizzAkuten.Fakes
{
    class FakeLookupNormailizer : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            throw new NotImplementedException();
        }
    }
}
