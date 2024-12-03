using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
            var HangXe = _context.HangXe.Where(hx => hx.IsDeleted == false).Select(hx => hx).ToList();
            return View(HangXe.ToList());
        }

        public IActionResult LoadDongXe(string TenHangXe, string MauSac, string DongCo, int DoiXe, long giaMin, long giaMax)
        {
            return ViewComponent("LoadDongXe", new { TenHangXe = TenHangXe, MauSac = MauSac, DongCo = DongCo, DoiXe = DoiXe, giaMin = giaMin, giaMax = giaMax });
        }

        public IActionResult GetCarDetails(long dongxeid, int doixe, string dongco)
        {
            var dx = _context.DongXe.Find(dongxeid);
            var hx = _context.HangXe.Where(h => h.HangXeID.Equals(dx.HangXeID)).FirstOrDefault();
            CarTypeModel car = new CarTypeModel
            {
                HangXeID = hx.HangXeID,
                TenHangXe = hx.TenHangXe,
                DongXeID = dongxeid,
                TenDongXe = dx.TenDongXe,
                DoiXe = doixe,
                DongCo = dongco,
                MauSacKhaDung = _context.CarType.Where(c => c.DoiXe.Equals(doixe) && c.DongXeID.Equals(dongxeid) && c.DongCo.Equals(dongco)).Select(c => c.MauSac).Distinct().ToList()
            };

            if (car == null)
            {
                return Content("Không tìm thấy thông tin xe.");
            }

            return PartialView("_CarDetails", car); // Trả về PartialView hiển thị chi tiết
        }

        public CarTypeModel GetCarDetailsWithColor(long dongxeid, int doixe, string dongco, string mauSac)
        {
            var dx = _context.DongXe.Where(d => d.DongXeID.Equals(dongxeid)).FirstOrDefault();
            var hx = _context.HangXe.Where(h => h.HangXeID.Equals(dx.HangXeID)).FirstOrDefault();
            var car = _context.CarType.Where(c => c.DongXeID.Equals(dongxeid) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco) && c.MauSac.Equals(mauSac)).FirstOrDefault();
            CarTypeModel cartype = new CarTypeModel
            {
                CarTypeID = car.CarTypeID,
                TenHangXe = hx.TenHangXe,
                HangXeID = hx.HangXeID,
                DongXeID = car.DongXeID,
                TenDongXe = dx.TenDongXe,
                DoiXe = car.DoiXe,
                DongCo = car.DongCo,
                GiaBan = car.GiaBan,
                CarImage = car.CarImage,
                SoLuong = _context.Car.Where(c => c.CarTypeID.Equals(car.CarTypeID) && c.TransactionID == null).Count()
            };

            return cartype;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
