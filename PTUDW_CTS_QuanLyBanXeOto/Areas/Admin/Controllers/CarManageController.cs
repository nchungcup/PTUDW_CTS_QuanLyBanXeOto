using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
using static System.Net.WebRequestMethods;
using System.Xml.Serialization;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers
{
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

        public IActionResult CarBrandManage()
        {
            //Lấy dữ liệu từ sql
            var carinfoList = (from hx in _context.HangXe
                               where hx.IsDeleted == false
                               select new HangXeModel
                               {
                                   HangXeID = hx.HangXeID,
                                   TenHangXe = hx.TenHangXe,
                                   XuatXu = hx.XuatXu,
                                   LogoImage = hx.LogoImage
                               }).ToList();

            return View(carinfoList);
        }

        [HttpPost]
        public async Task<IActionResult> AddHangXe(HangXeModel hxModel, IFormFile addLogoImage)
        {
            var filePath = "";
            var fileName = hxModel.TenHangXe + "_Logo";

            if (addLogoImage != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Logo");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await addLogoImage.CopyToAsync(stream);
                }
            }

            var hx = new HangXeDetail
            {
                TenHangXe = hxModel.TenHangXe,
                XuatXu = hxModel.XuatXu,
                LogoImage = fileName,
                IsDeleted = false
            };

            _context.HangXe.Add(hx);
            _context.SaveChanges();
            TempData["alertMessage"] = "Thêm hãng xe thành công!";
            return RedirectToAction("CarBrandManage", "CarManage");
        }

        [HttpPost]
        public async Task<IActionResult> EditHangXe(HangXeModel hxModel, IFormFile editLogoImage)
        {
            var hx = _context.HangXe.FirstOrDefault(h => h.HangXeID.Equals(hxModel.HangXeID) && h.IsDeleted == false);
            hx.TenHangXe = hxModel.TenHangXe;
            hx.XuatXu = hxModel.XuatXu;

            if (editLogoImage != null)
            {
                var filePath = "";
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Logo");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = hxModel.TenHangXe + "_Logo";

                filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await editLogoImage.CopyToAsync(stream);
                }
                hx.LogoImage = fileName;
            }

            _context.HangXe.Update(hx);
            _context.SaveChanges();
            TempData["alertMessage"] = "Sửa hãng xe thành công!";
            return RedirectToAction("CarBrandManage", "CarManage");
        }

        [HttpPost]
        public IActionResult DeleteHangXe(HangXeModel hxModel)
        {
            var hx = _context.HangXe.FirstOrDefault(h => h.HangXeID.Equals(hxModel.HangXeID));
            hx.IsDeleted = true;
            _context.HangXe.Update(hx);
            _context.SaveChanges();
            TempData["alertMessage"] = "Xóa hãng xe thành công!";
            return RedirectToAction("CarBrandManage", "CarManage");
        }

        [HttpGet]
        public IActionResult CarModelManage(long id)
        {
            var dx = (from d in _context.DongXe
                      where d.IsDeleted == false
                      where d.HangXeID.Equals(id)
                      select new DongXeModel
                      {
                          DongXeID = d.DongXeID,
                          TenDongXe = d.TenDongXe,
                          NoiSanXuat = d.NoiSanXuat
                      }).ToList();

            var carinfoList = (from hx in _context.HangXe
                               where hx.HangXeID.Equals(id)
                               select new HangXeModel
                               {
                                   HangXeID = hx.HangXeID,
                                   TenHangXe = hx.TenHangXe,
                                   XuatXu = hx.XuatXu,
                                   LogoImage = hx.LogoImage,
                                   ListDongXe = dx
                               }).FirstOrDefault();

            return View(carinfoList);
        }

        [HttpPost]
        public IActionResult AddDongXe(DongXeModel dxModel)
        {
            var dx = new DongXeDetail
            {
                HangXeID = dxModel.HangXeID,
                TenDongXe = dxModel.TenDongXe,
                NoiSanXuat = dxModel.NoiSanXuat,
                IsDeleted = false
            };

            _context.DongXe.Add(dx);
            _context.SaveChanges();
            TempData["alertMessage"] = "Thêm dòng xe thành công!";
            return RedirectToAction("CarModelManage", "CarManage", new { id = dxModel.HangXeID });
        }

        [HttpPost]
        public IActionResult EditDongXe(DongXeModel dxModel)
        {
            var dx = _context.DongXe.FirstOrDefault(h => h.DongXeID.Equals(dxModel.DongXeID) && h.IsDeleted == false);
            dx.TenDongXe = dxModel.TenDongXe;
            dx.NoiSanXuat = dxModel.NoiSanXuat;
            _context.DongXe.Update(dx);
            _context.SaveChanges();
            TempData["alertMessage"] = "Sửa dòng xe thành công!";
            return RedirectToAction("CarModelManage", "CarManage", new { id = dxModel.HangXeID });
        }

        [HttpPost]
        public IActionResult DeleteDongXe(DongXeModel dxModel)
        {
            var dx = _context.DongXe.FirstOrDefault(h => h.DongXeID.Equals(dxModel.DongXeID));
            dx.IsDeleted = true;
            _context.DongXe.Update(dx);
            _context.SaveChanges();
            TempData["alertMessage"] = "Xóa dòng xe thành công!";
            return RedirectToAction("CarModelManage", "CarManage", new { id = dxModel.HangXeID });
        }

        [HttpGet]
        public IActionResult CarTypeManage(long id)
        {
            var dongXe = _context.DongXe.FirstOrDefault(dx => dx.DongXeID.Equals(id) && dx.IsDeleted == false);
            var hangXe = _context.HangXe.FirstOrDefault(hx => hx.HangXeID.Equals(dongXe.HangXeID) && hx.IsDeleted == false);
            var carinfoList = (from ctp in _context.CarType
                               where ctp.DongXeID.Equals(id) && ctp.IsDeleted == false
                               select new CarTypeModel
                               {
                                   CarTypeID = ctp.CarTypeID,
                                   HangXeID = hangXe.HangXeID,
                                   TenHangXe = hangXe.TenHangXe,
                                   DongXeID = dongXe.DongXeID,
                                   TenDongXe = dongXe.TenDongXe,
                                   DoiXe = ctp.DoiXe,
                                   MauSac = ctp.MauSac,
                                   DongCo = ctp.DongCo,
                                   GiaNhap = ctp.GiaNhap,
                                   GiaBan = ctp.GiaBan,
                                   CarImage = ctp.CarImage,
                                   SoLuong = _context.Car.Where(c => c.CarTypeID.Equals(ctp.CarTypeID) && c.TransactionID == null && c.IsDeleted == false).Count(),
                                   ListHangXe = (from hx in _context.HangXe
                                                 where hx.IsDeleted == false
                                                 select new HangXeModel
                                                 {
                                                     HangXeID = hx.HangXeID,
                                                     TenHangXe = hx.TenHangXe,
                                                     XuatXu = hx.XuatXu,
                                                     LogoImage = hx.LogoImage,
                                                     ListDongXe = (from dx in _context.DongXe
                                                                   where dx.HangXeID.Equals(hx.HangXeID) && dx.IsDeleted == false
                                                                   select new DongXeModel
                                                                   {
                                                                       DongXeID = dx.DongXeID,
                                                                       TenDongXe = dx.TenDongXe,
                                                                       NoiSanXuat = dx.NoiSanXuat
                                                                   }).ToList()
                                                 }).ToList()
                               }).ToList();

            return View(carinfoList);
        }

        [HttpGet]
        public IActionResult AddCarType()
        {
            var listofHangXe = _context.HangXe.Where(hx => hx.IsDeleted == false).Select(hx => new HangXeModel { HangXeID = hx.HangXeID, TenHangXe = hx.TenHangXe }).ToList();
            var listHangXe = new CarTypeModel
            {
                ListHangXe = listofHangXe
            };
            return View(listHangXe);
        }

        [HttpGet]
        public List<DongXeDetail> loadDongXe(string TenHangXe)
        {
            var hx = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe) && hx.IsDeleted == false).FirstOrDefault();
            var dx = _context.DongXe.Where(dx => dx.HangXeID.Equals(hx.HangXeID) && dx.IsDeleted == false).Select(dx => new DongXeDetail { DongXeID = dx.DongXeID, TenDongXe = dx.TenDongXe }).ToList();
            return dx;
        }

        [HttpPost]
        public async Task<IActionResult> AddCarType(CarTypeModel _carmanage, IList<IFormFile> images)
        {
            //var VINduplicate = _context.Car.Where(c => c.VIN.Equals(_carmanage.VIN)).ToList();
            //if (VINduplicate.Count() > 0)
            //{
            //    TempData["alertMessage"] = "Mã VIN này đã tồn tại!";
            //    return RedirectToAction("Add", "CarManage");
            //}
            //else if (_context.User.Where(u => u.TypeID != 1 && u.Username.Equals(_carmanage.UsernameNguoiNhap)).Select(u => u.UserID).ToList().Count() > 0)
            //{
            //    TempData["alertMessage"] = "Tài khoản này không đủ quyền!";
            //    return RedirectToAction("Add", "CarManage");
            //}
            //else if (_context.User.Where(u => u.Username.Equals(_carmanage.UsernameNguoiNhap)).Select(u => u.UserID).ToList().Count() == 0)
            //{
            //    TempData["alertMessage"] = "Tài khoản này không tồn tại!";
            //    return RedirectToAction("Add", "CarManage", new { area = "Admin" });
            //}
            var dongXeID = _context.DongXe.Where(dx => dx.TenDongXe.Equals(_carmanage.TenDongXe) && dx.IsDeleted == false).Select(dx => dx.DongXeID).FirstOrDefault();
            var carType = _context.CarType.Where(ctp => ctp.DongXeID.Equals(dongXeID)
                && ctp.DoiXe.Equals(_carmanage.DoiXe)
                && ctp.MauSac.Equals(_carmanage.MauSac)
                && ctp.DongCo.Equals(_carmanage.DongCo)
                && ctp.IsDeleted == false).FirstOrDefault();
            if (carType != null)
            {
                TempData["alertMessage"] = "Mẫu xe này đã tồn tại!";
                return RedirectToAction("Add", "CarManage");
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
                        var fileName = Path.GetFileNameWithoutExtension(image.FileName) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(image.FileName);
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


                carType = new CarType
                {
                    DongXeID = dongXeID,
                    DoiXe = _carmanage.DoiXe,
                    MauSac = _carmanage.MauSac,
                    DongCo = _carmanage.DongCo,
                    CarImage = imagePathString,
                    GiaNhap = _carmanage.GiaNhap,
                    GiaBan = _carmanage.GiaBan,
                    IsDeleted = false
                };
                _context.CarType.Add(carType);
                await _context.SaveChangesAsync();

                TempData["alertMessage"] = "Thêm mẫu xe thành công!";
                return RedirectToAction("CarBrandManage", "CarManage");
            }
        }

        [HttpGet]
        public IActionResult DeleteCarType(long? id)
        {
            var ctp = _context.CarType.Where(ctp => ctp.CarTypeID.Equals(id) && ctp.IsDeleted == false).FirstOrDefault();
            var dx = _context.DongXe.Where(dx => dx.DongXeID.Equals(ctp.DongXeID) && dx.IsDeleted == false).FirstOrDefault();
            var hx = _context.HangXe.Where(hx => hx.HangXeID.Equals(dx.HangXeID) && hx.IsDeleted == false).FirstOrDefault();
            var cm = new CarTypeModel
            {
                CarTypeID = ctp.CarTypeID,
                TenHangXe = hx.TenHangXe,
                TenDongXe = dx.TenDongXe,
                DoiXe = ctp.DoiXe,
                MauSac = ctp.MauSac,
                DongCo = ctp.DongCo,
                CarImage = ctp.CarImage
            };
            return View(cm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCarType(CarTypeModel _carmanage)
        {
            var delCar = _context.CarType.Find(_carmanage.CarTypeID);
            delCar.IsDeleted = true;
            _context.CarType.Update(delCar);
            await _context.SaveChangesAsync();
            TempData["alertMessage"] = "Xóa mẫu xe thành công!";
            return RedirectToAction("CarBrandManage", "CarManage");
        }

        [HttpGet]
        public IActionResult EditCarType(long? id)
        {
            var ctp = _context.CarType.Where(ctp => ctp.CarTypeID.Equals(id) && ctp.IsDeleted == false).FirstOrDefault();
            var dx = _context.DongXe.Where(d => d.DongXeID.Equals(ctp.DongXeID) && d.IsDeleted == false).FirstOrDefault();
            var hx = _context.HangXe.Where(h => h.HangXeID.Equals(dx.HangXeID) && h.IsDeleted == false).FirstOrDefault();
            var hxList = _context.HangXe.Where(hx => hx.IsDeleted == false).Select(hx => new HangXeModel { HangXeID = hx.HangXeID, TenHangXe = hx.TenHangXe }).ToList();
            var cm = new CarTypeModel
            {
                CarTypeID = ctp.CarTypeID,
                DoiXe = ctp.DoiXe,
                MauSac = ctp.MauSac,
                DongCo = ctp.DongCo,
                GiaNhap = ctp.GiaNhap,
                GiaBan = ctp.GiaBan,
                CarImage = ctp.CarImage,
                HangXeID = hx.HangXeID,
                TenHangXe = hx.TenHangXe,
                DongXeID = ctp.DongXeID,
                TenDongXe = dx.TenDongXe,
                ListHangXe = hxList
            };

            return View(cm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCarType(CarTypeModel _carmanage, IList<IFormFile> images)
        {
            var dongXeID = _context.DongXe.Where(dx => dx.TenDongXe.Equals(_carmanage.TenDongXe) && dx.IsDeleted == false).Select(dx => dx.DongXeID).FirstOrDefault();
            var carType = _context.CarType.Where(ctp => ctp.CarTypeID.Equals(_carmanage.CarTypeID) && ctp.IsDeleted == false).FirstOrDefault();

            var dupCarType = _context.CarType.Where(ctp => ctp.DongXeID.Equals(dongXeID)
            && ctp.DoiXe.Equals(_carmanage.DoiXe)
            && ctp.MauSac.Equals(_carmanage.MauSac)
            && ctp.DongCo.Equals(_carmanage.DongCo)
            && ctp.CarTypeID != _carmanage.CarTypeID
            && ctp.IsDeleted == false).FirstOrDefault();
            if (dupCarType == null)
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

                if (imagePaths.Any())
                {
                    carType.CarImage = string.Join(",", imagePaths);
                }
                carType.DoiXe = _carmanage.DoiXe;
                carType.MauSac = _carmanage.MauSac;
                carType.DongCo = _carmanage.DongCo;
                carType.DongXeID = dongXeID;
                carType.GiaNhap = _carmanage.GiaNhap;
                carType.GiaBan = _carmanage.GiaBan;
                _context.CarType.Update(carType);
                await _context.SaveChangesAsync();

                TempData["alertMessage"] = "Cập nhật mẫu xe thành công!";
            }
            else
            {
                TempData["alertMessage"] = "Mẫu xe đã tồn tại!";
            }

            return RedirectToAction("CarBrandManage", "CarManage");
        }

        [HttpPost]
        public IActionResult UpdateCar(int carTypeId, List<string> vin)
        {
            var vinCars = _context.Car.Where(c => c.CarTypeID.Equals(carTypeId) && c.IsDeleted == false && c.TransactionID == null).ToList();

            if (vin == null || !vin.Any())
            {
                return Json(new { success = false, message = "Danh sách mã VIN không được để trống." });
            }

            foreach (var vinCode in vin)
            {
                if (string.IsNullOrWhiteSpace(vinCode))
                {
                    return Json(new { success = false, message = "Mã VIN không hợp lệ." });
                }
            }

            foreach (var vinCar in vinCars)
            {
                vinCar.IsDeleted = true;
                _context.Car.Update(vinCar);
                _context.SaveChanges();
            }   
            
            foreach (var vinCode in vin)
            {
                _context.Car.Add(new Car
                {
                    CarTypeID = carTypeId,
                    VIN = vinCode,
                    IsDeleted = false
                });
            }

            _context.SaveChanges();

            return Json(new { success = true, message = "Thêm mã VIN thành công!" });
        }

        [HttpGet]
        public IActionResult GetVinList(int carTypeId)
        {
            var vinList = _context.Car
                .Where(c => c.CarTypeID == carTypeId && c.IsDeleted == false)
                .Select(c => c.VIN)
                .ToList();

            return Json(new { success = true, vins = vinList });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
