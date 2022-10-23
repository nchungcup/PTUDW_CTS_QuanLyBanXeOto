using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Products()
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
