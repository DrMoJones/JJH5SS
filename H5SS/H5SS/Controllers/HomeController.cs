using H5SS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using H5SS.Areas.Identity.Codes;
using Microsoft.AspNetCore.DataProtection;
using H5SS.Codes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace H5SS.Controllers
{

    public class HomeController : Controller
    {
        //Værdierne du kan tage fat i (Referencerne)
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserRoleHandler _userRoleHandler;
        private readonly IDataProtector _dataProtector;
        private readonly CryptoEx _cryptoEx;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(
            //Tilegning af referencen 
            ILogger<HomeController> logger, 
            IServiceProvider serviceProvider, 
            UserRoleHandler userRoleHandler,
            IDataProtectionProvider dataProtector,
            CryptoEx cryptoEx,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _userRoleHandler = userRoleHandler;
            _dataProtector = dataProtector.CreateProtector("MinNoegle");
            _cryptoEx = cryptoEx;
            _signInManager = signInManager;

        }

        [Authorize("RequiredAuthenticatedUser")]
        public async Task<IActionResult> Index()
        {

            #region Testing
            var UserManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var User = UserManager.GetUserName(this.User);
            //checking they exists
            var u = _signInManager.IsSignedIn(this.User);
            Console.WriteLine(User);

            string Text1 = _cryptoEx.Encrypt("dnogeniawniodaw", _dataProtector);
            string Text2 = _cryptoEx.Decrypt(Text1, _dataProtector);


            #endregion


            return View();
        }

        [Authorize(Policy = "RequireAdminUser")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
