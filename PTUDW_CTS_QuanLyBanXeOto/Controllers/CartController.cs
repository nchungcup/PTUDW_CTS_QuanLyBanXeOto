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
                                join c in _context.Car on ca.CarID equals c.CarID
                                join dx in _context.DongXe on c.DongXeID equals dx.DongXeID
                                group c by new { c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                select new CartView
                                { 
                                    DongXeID = groupc.Key.DongXeID,
                                    CarImage = groupc.Key.CarImage,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    MauSac = groupc.Key.MauSac,
                                    GiaBan = groupc.Key.GiaBan,
                                    SoLuong = groupc.Count()
                                }).ToList();
                return View(cartview);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
        public async Task<IActionResult> AddCart(int dxid, string mausac, int doixe, string dongco)
        {
            var Soluongcon = (from c in _context.Car
                              join ct in _context.CarTrans on c.CarID equals ct.CarID
                              where c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco) && ct.TransID == null 
                              select c.CarID
                              ).Count();
            var clientses = HttpContext.Session.GetString("client");
            if (clientses == null)
            {
                return RedirectToAction("Signin", "SigninSignup");
            }
            else
            {
                var cartcheck = (from ca in _context.Cart
                                 join u in _context.User on ca.UserID equals u.UserID
                                 join c in _context.Car on ca.CarID equals c.CarID
                                 join ct in _context.CarTrans on c.CarID equals ct.CarID
                                 where clientses == u.Username && c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco)
                                 select new Cart
                                 {
                                     CartID = ca.CartID,
                                     CarID = c.CarID,
                                     UserID = u.UserID,
                                 }).ToList();
                if (Soluongcon > cartcheck.Count())
                {
                    var addcar = (from c in _context.Car
                                  join ct in _context.CarTrans on c.CarID equals ct.CarID
                                  where c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco) && ct.TransID == null
                                  select new Car
                                  {
                                      CarID = c.CarID
                                  }
                                  ).FirstOrDefault();
                    var addcart = new Cart
                                  {
                                      UserID = _context.User.Where(u => u.Username.Equals(clientses)).Select(u => u.UserID).FirstOrDefault(),
                                      CarID = addcar.CarID
                                  };
                    _context.Cart.Add(addcart);
                    await _context.SaveChangesAsync();
                    TempData["alertMessage"] = "Add To Cart - Success!";
                    return RedirectToAction("CartList", "Cart");
                }
                else
                    {
                        TempData["alertMessage"] = "This Car Is Temporarily Out Of Stock, Please Wait!";
                        return RedirectToAction("Products", "Products");
                    }    
            }
        }
        public async Task<IActionResult> Plus(int dxid, string mausac, int doixe, string dongco)
        {
            var Soluongcon = (from c in _context.Car
                              join ct in _context.CarTrans on c.CarID equals ct.CarID
                              where c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco) && ct.TransID == null
                              select c.CarID
                              ).Count();
            var clientses = HttpContext.Session.GetString("client");
            if (clientses == null)
            {
                return RedirectToAction("Signin", "SigninSignup");
            }
            else
            {
                var cartcheck = (from ca in _context.Cart
                                 join u in _context.User on ca.UserID equals u.UserID
                                 join c in _context.Car on ca.CarID equals c.CarID
                                 where clientses == u.Username && c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco)
                                 select new Cart
                                 {
                                     CartID = ca.CartID,
                                     CarID = c.CarID,
                                     UserID = u.UserID,
                                 }).ToList();
                if (Soluongcon > cartcheck.Count())
                {
                    var addcar = _context.Car.Where(c => c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco)).FirstOrDefault();
                    var addcart = (from ca in _context.Cart
                                   join u in _context.User on ca.UserID equals u.UserID
                                   where u.Username == clientses
                                   select new Cart
                                   {
                                       UserID = u.UserID,
                                       CarID = addcar.CarID
                                   }).FirstOrDefault();
                    _context.Cart.Add(addcart);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CartList", "Cart");
                }
                else
                {
                    TempData["alertMessage"] = "This Car Is Temporarily Out Of Stock, Please Wait!";
                    return RedirectToAction("CartList", "Cart");
                }
            }
        }
        public async Task<IActionResult> Minus(int dxid, string mausac, int doixe, string dongco)
        {
            var clientses = HttpContext.Session.GetString("client");
            if (clientses == null)
            {
                return RedirectToAction("Signin", "SigninSignup");
            }
            else
            {
                var cartcheck1 = (from ca in _context.Cart
                                 join u in _context.User on ca.UserID equals u.UserID
                                 join c in _context.Car on ca.CarID equals c.CarID
                                 join ct in _context.CarTrans on c.CarID equals ct.CarID
                                 where clientses == u.Username && c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco) && ct.TransID == null
                                 select new Cart
                                 {
                                     CartID = ca.CartID,
                                     CarID = ca.CarID,
                                     UserID = ca.UserID,
                                 }).ToList();
                var cartcheck2 = (from ca in _context.Cart
                                 join u in _context.User on ca.UserID equals u.UserID
                                 join c in _context.Car on ca.CarID equals c.CarID
                                 join ct in _context.CarTrans on c.CarID equals ct.CarID
                                 where clientses == u.Username && c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco) && ct.TransID != null
                                 select new Cart
                                 {
                                     CartID = ca.CartID,
                                     CarID = ca.CarID,
                                     UserID = ca.UserID,
                                 }).ToList();
                if (cartcheck2.Count() > 0)
                {
                    _context.Cart.Remove(cartcheck2.FirstOrDefault());
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CartList", "Cart");
                }
                else
                {
                    _context.Cart.Remove(cartcheck1.FirstOrDefault());
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CartList", "Cart");
                }
            }
        }
        public async Task<IActionResult> Remove(int dxid, string mausac, int doixe, string dongco)
        {
            var clientses = HttpContext.Session.GetString("client");
            var us = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
            var cartcheck = (from ca in _context.Cart
                             join c in _context.Car on ca.CarID equals c.CarID
                             where ca.UserID == us.UserID && c.DongXeID.Equals(dxid) && c.MauSac.Equals(mausac) && c.DoiXe.Equals(doixe) && c.DongCo.Equals(dongco)
                             select new Cart
                             {
                                 CartID = ca.CartID,
                                 CarID = c.CarID,
                                 UserID = us.UserID,
                             });
            _context.Cart.RemoveRange(cartcheck);
            await _context.SaveChangesAsync();
            return RedirectToAction("CartList", "Cart");
        }
        public async Task<IActionResult> ThanhToan(int tongtien, int thue, List<CartView> cartview)
        {
            var clientses = HttpContext.Session.GetString("client");
            var us = _context.User.Where(u => u.Username.Equals(clientses)).FirstOrDefault();
            var check = _context.Cart.Where(c => c.UserID.Equals(us.UserID)).Count();
            if (check > 0)
            {
                foreach (var cv in cartview)
                {
                    var cartcheck1 = (from ca in _context.Cart
                                      join c in _context.Car on ca.CarID equals c.CarID
                                      join ct in _context.CarTrans on c.CarID equals ct.CarID
                                      where ca.UserID == us.UserID && c.DongXeID.Equals(cv.DongXeID) && c.MauSac.Equals(cv.MauSac) && c.DoiXe.Equals(cv.DoiXe) && c.DongCo.Equals(cv.DongCo) && ct.TransID.Equals(null)
                                      select new Cart
                                      {
                                          CartID = ca.CartID,
                                          CarID = ca.CarID,
                                          UserID = ca.UserID,
                                      }).ToList();
                    var cartcheck2 = (from ca in _context.Cart
                                      join c in _context.Car on ca.CarID equals c.CarID
                                      join ct in _context.CarTrans on c.CarID equals ct.CarID
                                      where ca.UserID == us.UserID && c.DongXeID.Equals(cv.DongXeID) && c.MauSac.Equals(cv.MauSac) && c.DoiXe.Equals(cv.DoiXe) && c.DongCo.Equals(cv.DongCo) && ct.TransID != null
                                      select new Cart
                                      {
                                          CartID = ca.CartID,
                                          CarID = ca.CarID,
                                          UserID = ca.UserID,
                                      }).ToList();
                    if (cartcheck2.Count() > 0)
                    {
                        var Soluongcon = (from c in _context.Car
                                          join ct in _context.CarTrans on c.CarID equals ct.CarID
                                          where c.DongXeID.Equals(cv.DongXeID) && c.MauSac.Equals(cv.MauSac) && c.DoiXe.Equals(cv.DoiXe) && c.DongCo.Equals(cv.DongCo) && ct.TransID == null
                                          select c.CarID).Count();
                        if (Soluongcon > cartcheck2.Count())
                        {
                            var addcar = (from c in _context.Car
                                          join ct in _context.CarTrans on c.CarID equals ct.CarID
                                          where c.DongXeID.Equals(cv.DongXeID) && c.MauSac.Equals(cv.MauSac) && c.DoiXe.Equals(cv.DoiXe) && c.DongCo.Equals(cv.DongCo) && ct.TransID == null
                                          select c.CarID).Take(cartcheck2.Count()).ToList();
                            foreach (var cc in cartcheck2)
                            {
                                _context.Cart.Remove(cc);
                                await _context.SaveChangesAsync();
                            }
                            foreach (var ac in addcar)
                            {
                                var addcart = (from ca in _context.Cart
                                               where ca.UserID == us.UserID
                                               select new Cart
                                               {
                                                   UserID = us.UserID,
                                                   CarID = ac
                                               }).FirstOrDefault();
                                _context.Cart.Add(addcart);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var addcar = (from c in _context.Car
                                          join ct in _context.CarTrans on c.CarID equals ct.CarID
                                          where c.DongXeID.Equals(cv.DongXeID) && c.MauSac.Equals(cv.MauSac) && c.DoiXe.Equals(cv.DoiXe) && c.DongCo.Equals(cv.DongCo) && ct.TransID == null
                                          select c.CarID).ToList();
                            foreach (var cc in cartcheck2)
                            {
                                _context.Cart.Remove(cc);
                                await _context.SaveChangesAsync();
                            }
                            foreach (var ac in addcar)
                            {
                                var addcart = (from ca in _context.Cart
                                               where ca.UserID == us.UserID
                                               select new Cart
                                               {
                                                   UserID = us.UserID,
                                                   CarID = ac
                                               }).FirstOrDefault();
                                _context.Cart.Add(addcart);
                                await _context.SaveChangesAsync();
                            }
                            TempData["ThieuXeMessage"] = cv.TenDongXe + " - " + cv.MauSac + " - " + cv.DoiXe.ToString() + " - " + cv.DongCo + ", Kho Thiếu " + (cartcheck2.Count() - Soluongcon).ToString();
                        }
                    }
                }
                var khachhangid = _context.User.Where(u => u.Username.Equals(clientses)).Select(u => u.UserID).FirstOrDefault();
                var cartid = _context.Cart.Where(ca => ca.UserID.Equals(khachhangid)).Select(ca => ca.CartID).ToList();
                var w = new Transaction
                {
                    KhachHangID = khachhangid,
                    NgayTaoDon = DateTime.Now,
                    ThanhToan = null,
                    TrangThai = "Waitting",
                    NguoiXuLyID = null,
                    NgayDuyet = null,
                    NguoiDuyetID = null,
                    ChietKhau = 0,
                    Thue = thue,
                    TongTien = tongtien
                };
                _context.Transaction.Add(w);
                await _context.SaveChangesAsync();
                var carid = (from ca in _context.Cart
                             where ca.UserID == us.UserID
                             select ca.CarID).ToList();
                foreach (var ca in carid)
                {
                    var ct = new Car_Trans
                    {
                        CarID = ca,
                        TransID = w.TransID
                    };
                    _context.CarTrans.Update(ct);
                    await _context.SaveChangesAsync();
                }
                var removecart = new List<Cart>();
                foreach (var ca in cartid)
                {
                    var remove = _context.Cart.Find(ca);
                    removecart.Add(remove);
                }
                foreach (var ca in removecart)
                {
                    _context.Cart.Remove(ca);
                    await _context.SaveChangesAsync();
                }
                TempData["alertMessage"] = "We Will Contact You As Soon As Possible, Please Wait!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["alertMessage"] = "Your Cart Is Empty! Please Add Product To Your Cart";
                return RedirectToAction("Products", "Products");
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

