using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace OidcDemo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = null)
        {

            return LocalRedirect($"/identity/account/login{Request.QueryString}");
        }

        public IActionResult Logout()
        {

            return LocalRedirect($"/identity/account/logout{Request.QueryString}");
        }


    }
}