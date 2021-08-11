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
        

        public HomeController(
            //Tilegning af referencen 
            ILogger<HomeController> logger, 
            IServiceProvider serviceProvider, 
            UserRoleHandler userRoleHandler,
            IDataProtectionProvider dataProtector,
            CryptoEx cryptoEx)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _userRoleHandler = userRoleHandler;
            _dataProtector = dataProtector.CreateProtector("MinNoegle");
            _cryptoEx = cryptoEx;

        }

        [Authorize("RequiredAuthenticatedUser")]
        public async Task<IActionResult> Index()
        {
            //await _userRoleHandler.CreateRole("jaa@dk.com", "Admin", _serviceProvider);
            Console.WriteLine(_cryptoEx.Encrypt("dnogeniawniodaw", _dataProtector));
            string dw = _cryptoEx.Encrypt("dnogeniawniodaw", _dataProtector);
            Console.WriteLine(_cryptoEx.Decrypt(dw, _dataProtector));
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
