using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers
{
    //Tạo controller cho phần quản lý xe
    [Area("Admin")]
    public class CarManageController : BaseController
    {
        private readonly ILogger<CarManageController> _logger;
        private readonly DataContext _context;

        public CarManageController(ILogger<CarManageController> logger, DataContext dataContext) : base()
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
                                join ct in _context.CarTrans on c.CarID equals ct.CarID
                                join t in _context.Transaction on ct.TransID equals t.TransID into tlj
                                from t in tlj.DefaultIfEmpty()
                                where ct.TransID == null || t.TrangThai != "Approved" 
                                select new CarManage
                                {
                                    CarID = c.CarID,
                                    VIN = c.VIN,
                                    TenHangXe = hx.TenHangXe,
                                    HangXeID = hx.HangXeID,
                                    TenDongXe = dx.TenDongXe,
                                    DongXeID = dx.DongXeID,
                                    DoiXe = c.DoiXe,
                                    MauSac = c.MauSac,
                                    DongCo = c.DongCo,
                                    GiaNhap = c.GiaNhap,
                                    GiaBan = c.GiaBan,
                                    NgayNhap = c.NgayNhap,
                                    CarImage = c.CarImage,
                                    HoTenNguoiNhap = u.HoTen,
                                    TrangThai = ct.TransID == null ? "Còn Trong Kho" : t.TrangThai

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
                               join t in _context.Transaction on ct.TransID equals t.TransID into tlj
                               from t in tlj.DefaultIfEmpty()
                               where t.TrangThai == "Approved"
                               select new CarManage
                               {
                                   VIN = c.VIN,
                                   TenHangXe = hx.TenHangXe,
                                   TenDongXe = dx.TenDongXe,
                                   DoiXe = c.DoiXe,
                                   MauSac = c.MauSac,
                                   DongCo = c.DongCo,
                                   NgayNhap = c.NgayNhap,
                                   CarImage = c.CarImage,
                                   HoTenNguoiNhap = u.HoTen,
                                   TrangThai = t.TrangThai

                               }).ToList();
            //Trả về view Xe đã bán với dữ liệu là carinfoList
            return View(carinfoList);
        }

        public IActionResult Add()
        {
            var listofHangXe = _context.HangXe.Select(hx => new HangXe { HangXeID = hx.HangXeID, TenHangXe = hx.TenHangXe }).ToList();
            var listHangXe = new CarManage
                                {
                                   ListHangXe = listofHangXe
                                };
            return View("Add", listHangXe);
        }

        [HttpGet]
        public List<DongXeDetail> loadDongXe(string TenHangXe)
        {
            var hx = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).FirstOrDefault();
            var dx = _context.DongXe.Where(dx => dx.HangXeID.Equals(hx.HangXeID)).Select(dx => new DongXeDetail { DongXeID = dx.DongXeID, TenDongXe = dx.TenDongXe }).ToList();
            return dx;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCar(CarManage _carmanage)
        {
            var VINduplicate = _context.Car.Where(c => c.VIN.Equals(_carmanage.VIN)).ToList();
            if (VINduplicate.Count() > 0)
            {
                TempData["alertMessage"] = "This VIN Already Exists On The System";
                return RedirectToAction("Add", "CarManage");
            }
            else if (_context.User.Where(u => u.TypeID != 1 && u.HoTen.Equals(_carmanage.HoTenNguoiNhap)).Select(u => u.UserID).ToList().Count() > 0)
            {
                TempData["alertMessage"] = "This User Does Not Have Permission To Perform This Action";
                return RedirectToAction("Add", "CarManage");
            }
            else if (_context.User.Where(u => u.HoTen.Equals(_carmanage.HoTenNguoiNhap)).Select(u => u.UserID).ToList().Count() == 0)
            {
                TempData["alertMessage"] = "This User Does Not Exist";
                return RedirectToAction("Add", "CarManage");
            }
            else
            {
                var c = new Car
                {
                    VIN = _carmanage.VIN,
                    DongXeID = _context.DongXe.Where(dx => dx.TenDongXe.Equals(_carmanage.TenDongXe)).Select(dx => dx.DongXeID).FirstOrDefault(),
                    DoiXe = _carmanage.DoiXe,
                    MauSac = _carmanage.MauSac,
                    DongCo = _carmanage.DongCo,
                    NgayNhap = _carmanage.NgayNhap,
                    NguoiNhapID = _context.User.Where(u => u.HoTen.Equals(_carmanage.HoTenNguoiNhap)).Select(u => u.UserID).FirstOrDefault(),
                    PhieuXuatID = null
                };
                var ct = new Car_Trans
                {
                    TransID = null
                };
                _context.Car.Add(c);
                await _context.SaveChangesAsync();
                _context.CarTrans.Add(ct);
                await _context.SaveChangesAsync();
                TempData["alertMessage"] = "Action Completed";
                return RedirectToAction("XeChuaBan", "CarManage");
            }
        }
        
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var c = _context.Car.Where(c => c.CarID.Equals(id)).FirstOrDefault();
            var dx = _context.DongXe.Where(dx => dx.DongXeID.Equals(c.DongXeID)).FirstOrDefault();
            var hx = _context.HangXe.Where(hx => hx.HangXeID.Equals(dx.HangXeID)).FirstOrDefault();
            var ct = _context.CarTrans.Where(ct => ct.CarID.Equals(c.CarID)).FirstOrDefault();
            var u = _context.User.Where(u => u.UserID.Equals(c.NguoiNhapID)).FirstOrDefault();
            var cm = new CarManage
                      {
                          CarID = c.CarID,
                          VIN = c.VIN,
                          TenHangXe = hx.TenHangXe,
                          TenDongXe = dx.TenDongXe,
                          DoiXe = c.DoiXe,
                          MauSac = c.MauSac,
                          DongCo = c.DongCo,
                          NgayNhap = c.NgayNhap,
                          CarImage = c.CarImage,
                          HoTenNguoiNhap = u.HoTen,
                          TrangThai = "Chưa Bán"

                      };
            return View("Delete", cm);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var delCar = _context.Car.Find(id);
            var delCarTrans = _context.CarTrans.Find(id);
            _context.CarTrans.Remove(delCarTrans);
            await _context.SaveChangesAsync();
            _context.Car.Remove(delCar);
            await _context.SaveChangesAsync();
            TempData["alertMessage"] = "Action Completed";
            return RedirectToAction("XeChuaBan", "CarManage");
        }
        
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var c = _context.Car.Where(c => c.CarID.Equals(id)).FirstOrDefault();
            var hx = _context.HangXe.Select(hx => new HangXe { HangXeID = hx.HangXeID, TenHangXe = hx.TenHangXe }).ToList();
            var ct = _context.CarTrans.Where(ct => ct.CarID.Equals(c.CarID)).FirstOrDefault();
            var u = _context.User.Where(u => u.UserID.Equals(c.NguoiNhapID)).FirstOrDefault();
            var cm = new CarManage
                      {
                          CarID = c.CarID,
                          VIN = c.VIN,
                          DoiXe = c.DoiXe,
                          MauSac = c.MauSac,
                          DongCo = c.DongCo,
                          NgayNhap = c.NgayNhap,
                          CarImage = c.CarImage,
                          HoTenNguoiNhap = u.HoTen,
                          TrangThai = "Còn Trong Kho",
                          ListHangXe = hx
                      };
            return View("Edit", cm);
        }    
        [HttpPost]
        public async Task<IActionResult> Edit(CarManage _cm)
        {
            var editCar = new Car
            {
                CarID = _cm.CarID,
                VIN = _cm.VIN,
                DongXeID = _context.DongXe.Where(dx => dx.TenDongXe.Equals(_cm.TenDongXe)).Select(dx => dx.DongXeID).FirstOrDefault(),
                DoiXe = _cm.DoiXe,
                MauSac = _cm.MauSac,
                DongCo = _cm.DongCo,
                CarImage = _cm.CarImage,
                PhieuXuatID = _cm.PhieuXuatID,
                NgayNhap = _cm.NgayNhap,
                NguoiNhapID = _context.User.Where(u => u.HoTen.Equals(_cm.HoTenNguoiNhap)).Select(u => u.UserID).FirstOrDefault()
            };
            _context.Car.Update(editCar);
            await _context.SaveChangesAsync();
            TempData["alertMessage"] = "Action Completed";
            return RedirectToAction("XeChuaBan", "CarManage");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
