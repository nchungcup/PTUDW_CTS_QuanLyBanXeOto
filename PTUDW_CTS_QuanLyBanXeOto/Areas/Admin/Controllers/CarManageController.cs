using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers
{
    //Tạo controller cho phần quản lý xe
    [Area("Admin")]
    public class CarManageController : Controller
    {
        private readonly ILogger<CarManageController> _logger;
        private readonly DataContext _context;

        public CarManageController(ILogger<CarManageController> logger, DataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }

        //Tạo method để hiện dữ liệu là những xe chưa bán
        public IActionResult XeChuaBan()
        {
            //Lấy dữ liệu từ sql
            var carinfoList = (from hx in _context.HangXe
                                join dx in _context.DongXe on hx.HangXeID equals dx.HangXeID
                                join c in _context.Car on dx.DongXeID equals c.DongXeID
                                join u in _context.User on c.NguoiNhapID equals u.UserID
                                join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                from ct in ctc.DefaultIfEmpty()
                                where ct.TransID == null
                               select new
                                {
                                    VIN = c.VIN,
                                    TenHangXe = hx.TenHangXe,
                                    TenDongXe = dx.TenDongXe,
                                    DoiXe = c.DoiXe,
                                    MauSac = c.MauSac,
                                    DongCo = c.DongCo,
                                    GiaNhap = c.GiaNhap,
                                    GiaBan = c.GiaBan,
                                    NgayNhap = c.NgayNhap,
                                    CarImage = c.CarImage,
                                    HoTenNguoiNhap = u.HoTen,
                                    TrangThai = "Chưa Bán"

                                }).AsEnumerable().Select(cm => new CarManage
                                {
                                    VIN = cm.VIN,
                                    TenHangXe = cm.TenHangXe,
                                    TenDongXe = cm.TenDongXe,
                                    DoiXe = cm.DoiXe,
                                    MauSac = cm.MauSac,
                                    DongCo = cm.DongCo,
                                    GiaNhap = cm.GiaNhap,
                                    GiaBan = cm.GiaBan,
                                    NgayNhap = cm.NgayNhap,
                                    CarImage = cm.CarImage,
                                    HoTenNguoiNhap = cm.HoTenNguoiNhap,
                                    TrangThai = cm.TrangThai
                                }).ToList();
            //Trả về view Xe chưa bán với dữ liệu là carinfoList
            return View(carinfoList);
        }

        //Tạo method để hiện dữ liệu là những xe đã bán
        public IActionResult XeDaBan()
        {
            //Lấy dữ liệu từ sql
            var carinfoList = (from hx in _context.HangXe
                               join dx in _context.DongXe on hx.HangXeID equals dx.HangXeID
                               join c in _context.Car on dx.DongXeID equals c.DongXeID
                               join u in _context.User on c.NguoiNhapID equals u.UserID
                               join ct in _context.CarTrans on c.CarID equals ct.CarID
                               join t in _context.Transaction on ct.TransID equals t.TransID
                               where ct.TransID != null
                               select new
                               {
                                   VIN = c.VIN,
                                   TenHangXe = hx.TenHangXe,
                                   TenDongXe = dx.TenDongXe,
                                   DoiXe = c.DoiXe,
                                   MauSac = c.MauSac,
                                   DongCo = c.DongCo,
                                   GiaNhap = c.GiaNhap,
                                   GiaBan = c.GiaBan,
                                   NgayNhap = c.NgayNhap,
                                   CarImage = c.CarImage,
                                   HoTenNguoiNhap = u.HoTen,
                                   TrangThai = "Đã Bán"

                               }).AsEnumerable().Select(cm => new CarManage
                               {
                                   VIN = cm.VIN,
                                   TenHangXe = cm.TenHangXe,
                                   TenDongXe = cm.TenDongXe,
                                   DoiXe = cm.DoiXe,
                                   MauSac = cm.MauSac,
                                   DongCo = cm.DongCo,
                                   GiaNhap = cm.GiaNhap,
                                   GiaBan = cm.GiaBan,
                                   NgayNhap = cm.NgayNhap,
                                   CarImage = cm.CarImage,
                                   HoTenNguoiNhap = cm.HoTenNguoiNhap,
                                   TrangThai = cm.TrangThai
                               }).ToList();
            //Trả về view Xe đã bán với dữ liệu là carinfoList
            return View(carinfoList);
        }

        //public async IActionResult Add(Car car)
        //{
        //    _context.Car
        //        return View()
        //}
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            { return NotFound(); }
            var c = _context.Car.Find(id);
            if( c == null)
            { return NotFound(); }
            return View(c);
        }    

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editCar = _context.Car.Find(id);
            if(editCar == null)
            { return NotFound(); }
            _context.Car.Update(editCar);
            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
