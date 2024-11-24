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
using System.IO;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Components.Forms;

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
                                    Username = u.Username,
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
                                   Username = u.Username,
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
        public async Task<IActionResult> AddNewCar(CarManage _carmanage, IList<IFormFile> images)
        {
            // Kiểm tra trùng VIN
            var VINduplicate = _context.Car.Where(c => c.VIN.Equals(_carmanage.VIN)).ToList();
            if (VINduplicate.Count() > 0)
            {
                TempData["alertMessage"] = "Mã VIN này đã tồn tại!";
                return RedirectToAction("Add", "CarManage");
            }
            else if (_context.User.Where(u => u.TypeID != 1 && u.Username.Equals(_carmanage.Username)).Select(u => u.UserID).ToList().Count() > 0)
            {
                TempData["alertMessage"] = "Tài khoản này không đủ quyền!";
                return RedirectToAction("Add", "CarManage");
            }
            else if (_context.User.Where(u => u.Username.Equals(_carmanage.Username)).Select(u => u.UserID).ToList().Count() == 0)
            {
                TempData["alertMessage"] = "Tài khoản này không tồn tại!";
                return RedirectToAction("Add", "CarManage", new { area = "Admin" });
            }
            else
            {
                List<string> imagePaths = new List<string>();  // Danh sách lưu đường dẫn ảnh
                if (images != null && images.Count > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Car");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var image in images)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(image.FileName) + "_" + _carmanage.Username + "_" + _carmanage.VIN + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(image.FileName);
                        imagePaths.Add(fileName);  // Lưu đường dẫn ảnh vào danh sách

                        var filePath = Path.Combine(uploadsFolder, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                    }
                }

                // Chuyển danh sách đường dẫn ảnh thành chuỗi phân cách bằng dấu phẩy (nếu cần)
                string imagePathString = string.Join(",", imagePaths);

                var c = new Car
                {
                    VIN = _carmanage.VIN,
                    DongXeID = _context.DongXe.Where(dx => dx.TenDongXe.Equals(_carmanage.TenDongXe)).Select(dx => dx.DongXeID).FirstOrDefault(),
                    DoiXe = _carmanage.DoiXe,
                    MauSac = _carmanage.MauSac,
                    DongCo = _carmanage.DongCo,
                    NgayNhap = _carmanage.NgayNhap,
                    GiaNhap = _carmanage.GiaNhap,
                    GiaBan = _carmanage.GiaBan,
                    NguoiNhapID = _context.User.Where(u => u.Username.Equals(_carmanage.Username)).Select(u => u.UserID).FirstOrDefault(),
                    PhieuXuatID = null,
                    CarImage = imagePathString // Lưu chuỗi đường dẫn ảnh vào cơ sở dữ liệu
                };
                _context.Car.Add(c);
                await _context.SaveChangesAsync();

                var ct = new Car_Trans
                {
                    CarID = c.CarID,
                    TransID = null
                };
                _context.CarTrans.Add(ct);
                await _context.SaveChangesAsync();

                var sameCars = _context.Car.Where(c => c.DongXeID.Equals(_carmanage.DongXeID)
                && c.DongCo.Equals(_carmanage.DongCo)
                && c.DoiXe.Equals(_carmanage.DoiXe)
                && c.MauSac.Equals(_carmanage.MauSac)).ToList();

                foreach (var car in  sameCars)
                {
                    car.GiaNhap = _carmanage.GiaNhap;
                    car.GiaBan = _carmanage.GiaBan;
                    car.CarImage = imagePathString;
                    _context.Car.Update(car);
                    await _context.SaveChangesAsync();
                }

                TempData["alertMessage"] = "Thêm xe " + c.VIN + " thành công!";
                return RedirectToAction("XeChuaBan", "CarManage");
            }
        }


        [HttpGet]
        public IActionResult Delete(long? id)
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
                          Username = u.Username,
                          TrangThai = "Chưa Bán"

                      };
            return View("Delete", cm);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
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
        public IActionResult Edit(long? id)
        {
            var c = _context.Car.Where(c => c.CarID.Equals(id)).FirstOrDefault();
            var dx = _context.DongXe.Where(d => d.DongXeID.Equals(c.DongXeID)).FirstOrDefault();
            var hx = _context.HangXe.Where(h => h.HangXeID.Equals(dx.HangXeID)).FirstOrDefault();
            var hxList = _context.HangXe.Select(hx => new HangXe { HangXeID = hx.HangXeID, TenHangXe = hx.TenHangXe }).ToList();
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
                          GiaNhap = c.GiaNhap,
                          GiaBan = c.GiaBan,
                          CarImage = c.CarImage,
                          Username = u.Username,
                          HangXeID = hx.HangXeID,
                          TenHangXe = hx.TenHangXe,
                          DongXeID = c.DongXeID,
                          TenDongXe = dx.TenDongXe,
                          TrangThai = "Còn Trong Kho",
                          ListHangXe = hxList
            };

            return View("Edit", cm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarManage _carmanage, IList<IFormFile> images)
        {
            var VINduplicate = _context.Car
                .Where(c => c.VIN.Equals(_carmanage.VIN) && c.CarID != _carmanage.CarID)
                .ToList();

            if (VINduplicate.Count() > 0)
            {
                TempData["alertMessage"] = "Tên đăng nhập đã tồn tại!";
                return RedirectToAction("Edit", "CarManage", new { id = _carmanage.CarID });
            }

            var editCar = await _context.Car
                .FirstOrDefaultAsync(c => c.CarID == _carmanage.CarID);

            if (editCar != null)
            {
                List<string> imagePaths = new List<string>();
                if (images != null && images.Count > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Car");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var image in images)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(image.FileName) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(image.FileName);
                        imagePaths.Add(fileName);  // Lưu đường dẫn ảnh vào danh sách

                        var filePath = Path.Combine(uploadsFolder, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                    }
                }

                // Nếu có ảnh mới, thay thế ảnh cũ
                if (imagePaths.Any())
                {
                    editCar.CarImage = string.Join(",", imagePaths);
                }

                // Cập nhật các trường khác của Car nếu cần
                editCar.VIN = _carmanage.VIN;
                editCar.DongXeID = _context.DongXe.Where(dx => dx.TenDongXe.Equals(_carmanage.TenDongXe)).Select(dx => dx.DongXeID).FirstOrDefault();
                editCar.DoiXe = _carmanage.DoiXe;
                editCar.MauSac = _carmanage.MauSac;
                editCar.DongCo = _carmanage.DongCo;
                editCar.PhieuXuatID = _carmanage.PhieuXuatID;
                editCar.GiaNhap = _carmanage.GiaNhap;
                editCar.NgayNhap = _carmanage.NgayNhap;
                editCar.GiaBan = _carmanage.GiaBan;
                editCar.NguoiNhapID = _context.User.Where(u => u.Username.Equals(_carmanage.Username)).Select(u => u.UserID).FirstOrDefault();

                // Cập nhật đối tượng Car trong cơ sở dữ liệu
                _context.Car.Update(editCar);
                await _context.SaveChangesAsync();

                var sameCars = _context.Car.Where(c => c.DongXeID.Equals(_carmanage.DongXeID)
                && c.DongCo.Equals(_carmanage.DongCo)
                && c.DoiXe.Equals(_carmanage.DoiXe)
                && c.MauSac.Equals(_carmanage.MauSac)).ToList();

                foreach (var car in sameCars)
                {

                    car.GiaNhap = _carmanage.GiaNhap;
                    car.GiaBan = _carmanage.GiaBan;
                    if (imagePaths.Any())
                    {
                        car.CarImage = string.Join(",", imagePaths);
                    }
                    _context.Car.Update(car);
                    await _context.SaveChangesAsync();
                }

                TempData["alertMessage"] = "Cập nhật xe " + editCar.VIN + " thành công!";
            }
            else
            {
                TempData["alertMessage"] = "Không tìm thấy xe!";
            }

            return RedirectToAction("XeChuaBan", "CarManage");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
