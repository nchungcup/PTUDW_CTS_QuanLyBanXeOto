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
        private readonly DataContext _context;

        public ProductsController(ILogger<ProductsController> logger, DataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }

        public IActionResult Products()
        {
            var HangXe = _context.HangXe.Select(hx => hx).ToList();
            return View(HangXe.ToList());
        }

        public IActionResult LoadDongXe(string TenHangXe, string MauSac, string DongCo)
        {
            return ViewComponent("LoadDongXe", new { TenHangXe = TenHangXe, MauSac = MauSac, DongCo = DongCo });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
