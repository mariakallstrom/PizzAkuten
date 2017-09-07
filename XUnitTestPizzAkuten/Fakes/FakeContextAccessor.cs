using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace XUnitTestPizzAkuten.Fakes
{
    class FakeContextAccessor: IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }
    }
}
