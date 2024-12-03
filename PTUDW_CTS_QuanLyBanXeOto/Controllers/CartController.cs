using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Controllers
{
    public class CartController : ClientBaseController
    {
        private readonly ILogger<CartController> _logger;
        private readonly DataContext _context;

        public CartController(ILogger<CartController> logger, DataContext dataContext) : base()
        {
            _logger = logger;
            _context = dataContext;
        }

        public IActionResult CartList()
        {
            var adminses = HttpContext.Session.GetString("username");
            var clientses = HttpContext.Session.GetString("client");
            if (adminses == null && clientses == null)
            {
                return RedirectToAction("Signin", "SigninSignup");
            }
            else if (adminses == null && clientses != null)
            {
                var us = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
                var cartview = (from ca in _context.Cart
                                where ca.UserID == us.UserID
                                join ctp in _context.CarType on ca.CarTypeID equals ctp.CarTypeID
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where ctp.IsDeleted == false && hx.IsDeleted == false && dx.IsDeleted == false
                                group ca by new { hx.HangXeID, hx.TenHangXe, ctp.DongXeID, dx.TenDongXe, ctp.MauSac, ctp.DoiXe, ctp.DongCo, ctp.GiaBan, ctp.CarImage, ca.SoLuong } into groupc
                                select new CartModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    CarImage = groupc.Key.CarImage,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    MauSac = groupc.Key.MauSac,
                                    GiaBan = groupc.Key.GiaBan,
                                    SoLuong = groupc.Key.SoLuong
                                }).ToList();
                foreach (var car in cartview)
                {
                    if (!string.IsNullOrEmpty(car.CarImage))
                    {
                        car.CarImage = car.CarImage.Split(',')[0]; // Lấy ảnh đầu tiên từ chuỗi CarImage
                    }
                }
                return View(cartview);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
        public async Task<IActionResult> AddCart(long dxid, string mausac, long doixe, string dongco)
        {
            var clientses = HttpContext.Session.GetString("client");
            if (clientses == null)
            {
                return RedirectToAction("Signin", "SigninSignup");
            }
            else
            {
                var user = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
                var addcartype = _context.CarType.Where(ctp => ctp.DongXeID.Equals(dxid) && ctp.MauSac.Equals(mausac) && ctp.DoiXe.Equals(doixe) && ctp.DongCo.Equals(dongco)).FirstOrDefault();
                var isInCart = _context.Cart.Where(c => c.UserID.Equals(user.UserID) && c.CarTypeID.Equals(addcartype.CarTypeID)).FirstOrDefault();
                if (isInCart == null)
                {
                    var addcart = new Cart
                    {
                        UserID = _context.User.Where(u => u.Username.Equals(clientses)).Select(u => u.UserID).FirstOrDefault(),
                        CarTypeID = addcartype.CarTypeID,
                        SoLuong = 1
                    };
                    _context.Cart.Add(addcart);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    isInCart.SoLuong = isInCart.SoLuong + 1;
                    _context.Cart.Update(isInCart);
                    await _context.SaveChangesAsync();
                }
                TempData["alertMessage"] = "Thêm vào giỏ hàng thành công!";
                return RedirectToAction("CartList", "Cart");
            }
        }
        public async Task<IActionResult> Plus(long dxid, string mausac, long doixe, string dongco)
        {
            var clientses = HttpContext.Session.GetString("client");
            if (clientses == null)
            {
                return RedirectToAction("Signin", "SigninSignup");
            }
            else
            {
                var user = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
                var carType = _context.CarType.Where(ctp => ctp.DongXeID.Equals(dxid) && ctp.MauSac.Equals(mausac) && ctp.DoiXe.Equals(doixe) && ctp.DongCo.Equals(dongco)).FirstOrDefault();
                var isInCart = _context.Cart.Where(c => c.UserID.Equals(user.UserID) && c.CarTypeID.Equals(carType.CarTypeID)).FirstOrDefault();
                if (isInCart == null)
                {
                    var addcart = new Cart
                    {
                        UserID = _context.User.Where(u => u.Username.Equals(clientses)).Select(u => u.UserID).FirstOrDefault(),
                        CarTypeID = carType.CarTypeID,
                        SoLuong = 1
                    };
                    _context.Cart.Add(addcart);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    isInCart.SoLuong = isInCart.SoLuong + 1;
                    _context.Cart.Update(isInCart);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("CartList", "Cart");
            }
        }
        public async Task<IActionResult> Minus(long dxid, string mausac, long doixe, string dongco)
        {
            var clientses = HttpContext.Session.GetString("client");
            if (clientses == null)
            {
                return RedirectToAction("Signin", "SigninSignup");
            }
            else
            {
                var user = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
                var addcartype = _context.CarType.Where(ctp => ctp.DongXeID.Equals(dxid) && ctp.MauSac.Equals(mausac) && ctp.DoiXe.Equals(doixe) && ctp.DongCo.Equals(dongco)).FirstOrDefault();
                var cart = _context.Cart.Where(c => c.UserID.Equals(user.UserID) && c.CarTypeID.Equals(addcartype.CarTypeID)).FirstOrDefault();
                cart.SoLuong = cart.SoLuong - 1;

                if (cart.SoLuong > 0)
                {
                    _context.Cart.Update(cart);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CartList", "Cart");
                }
                else
                {
                    _context.Cart.Remove(cart);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CartList", "Cart");
                }
            }
        }
        public async Task<IActionResult> Remove(long dxid, string mausac, long doixe, string dongco)
        {
            var clientses = HttpContext.Session.GetString("client");
            var user = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
            var carType = _context.CarType.Where(ctp => ctp.DongXeID.Equals(dxid) && ctp.MauSac.Equals(mausac) && ctp.DoiXe.Equals(doixe) && ctp.DongCo.Equals(dongco)).FirstOrDefault();
            var cart = _context.Cart.Where(c => c.UserID.Equals(user.UserID) && c.CarTypeID.Equals(carType.CarTypeID)).FirstOrDefault();
            _context.Cart.RemoveRange(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("CartList", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToan([FromBody] ThanhToanModel model)
        {
            var tongtien = model.TongTien;
            var cartview = model.CartView;

            if (cartview == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Giỏ hàng của bạn đang trống!"
                });
            }

            var clientses = HttpContext.Session.GetString("client");
            var us = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
            var cart = _context.Cart.Where(c => c.UserID.Equals(us.UserID)).Count();

            if (cart > 0)
            {
                foreach (var cv in cartview)
                {
                    var carType = _context.CarType.Where(ctp => ctp.DongXeID.Equals(cv.DongXeID) && ctp.MauSac.Equals(cv.MauSac) && ctp.DoiXe.Equals(cv.DoiXe) && ctp.DongCo.Equals(cv.DongCo)).FirstOrDefault();
                    var carCheck = (from c in _context.Car
                                    where c.CarTypeID.Equals(carType.CarTypeID) && c.TransactionID == null
                                    select c).ToList();
                    if (carCheck.Count() < cv.SoLuong)
                    {
                        return Json(new
                        {
                            success = false,
                            message = $"{cv.TenHangXe} {cv.TenDongXe} {cv.MauSac} {cv.DoiXe} {cv.DongCo} chỉ còn {carCheck.Count()} chiếc, vui lòng cập nhật lại số lượng!"
                        });
                    }
                }

                foreach (var cv in cartview)
                {
                    var carType = _context.CarType.Where(ctp => ctp.DongXeID.Equals(cv.DongXeID) && ctp.MauSac.Equals(cv.MauSac) && ctp.DoiXe.Equals(cv.DoiXe) && ctp.DongCo.Equals(cv.DongCo)).FirstOrDefault();
                    var carCheck = (from c in _context.Car
                                    where c.CarTypeID.Equals(carType.CarTypeID) && c.TransactionID == null
                                    select c).ToList();
                    if (carCheck.Count() >= cv.SoLuong)
                    {
                        var trans = new Transaction
                        {
                            KhachHangID = us.UserID,
                            NgayTaoDon = DateTime.Now,
                            ThanhToan = null,
                            TrangThai = "Chờ xác nhận",
                            NguoiXuLyID = null,
                            NgayDuyet = null,
                            NguoiDuyetID = null,
                            ChietKhau = 0,
                            TongTien = tongtien
                        };
                        _context.Transaction.Add(trans);
                        await _context.SaveChangesAsync();

                        var carsBought = carCheck.Take((int)cv.SoLuong);
                        foreach (var cb in carsBought)
                        {
                            cb.TransactionID = trans.TransID;
                            _context.Car.Update(cb);
                            await _context.SaveChangesAsync();
                        }

                        var cartForCarType = _context.Cart.Where(c => c.UserID.Equals(us.UserID) && c.CarTypeID.Equals(carType.CarTypeID)).FirstOrDefault();
                        _context.Cart.Remove(cartForCarType);
                        await _context.SaveChangesAsync();
                    }
                }

                return Json(new
                {
                    success = true,
                    message = "Vui lòng chờ cho đến khi chúng tôi liên lạc với bạn!",
                    redirectUrl = Url.Action("Index", "Home") // URL chuyển hướng sau thành công
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Giỏ hàng của bạn đang trống!"
                });
            }
        }

        public IActionResult TransHis()
        {
            var clientses = HttpContext.Session.GetString("client");
            var u = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
            var transhis = _context.Transaction.Where(t => t.KhachHangID.Equals(u.UserID)).OrderByDescending(t => t.NgayTaoDon).ToList();
            return View(transhis);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

