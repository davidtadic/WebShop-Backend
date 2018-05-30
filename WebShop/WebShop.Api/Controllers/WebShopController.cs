using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebShop.Api.Helpers;

namespace WebShop.Api.Controllers
{
    [Route("api/[controller]")]
    public class WebShopController : Controller
    {
        private readonly AuthCredentials _authCredentials;

        public WebShopController(IOptions<AuthCredentials> authCredentials)
        {
            _authCredentials = authCredentials.Value;
        }

        [HttpGet("TestMethod")]
        [Produces("application/json")]
        public object TestMethod()
        {
            string username = _authCredentials.Username;
            string password = _authCredentials.Password;

            return new { Username = username, Password = password};
        }

    }
}
