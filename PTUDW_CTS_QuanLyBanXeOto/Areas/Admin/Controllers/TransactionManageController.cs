using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TransactionManageController : BaseController
    {
        private readonly ILogger<TransactionManageController> _logger;
        private readonly DataContext _context;

        public TransactionManageController(ILogger<TransactionManageController> logger, DataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Waitting()
        {
            var w = (from trans in _context.Transaction
                     join u in _context.User on trans.KhachHangID equals u.UserID
                     where trans.TrangThai == "Chờ xác nhận"
                     select new TransView
                     {
                         TransID = trans.TransID,
                         KhachHangID = trans.KhachHangID,
                         HoTenKhachHang = u.HoTen,
                         SoDienThoai = u.SoDienThoai,
                         NgayTaoDon = trans.NgayTaoDon,
                         TrangThai = trans.TrangThai,
                     }).ToList();
            return View(w);
        }
        public IActionResult WaittingConfirm(long transid)
        {
            var trans = _context.Transaction.Where(t => t.TransID.Equals(transid)).FirstOrDefault();
            var u = _context.User.Where(u => u.UserID.Equals(trans.KhachHangID)).FirstOrDefault();
            var w = new TransView
            {
                TransID = trans.TransID,
                KhachHangID = trans.KhachHangID,
                HoTenKhachHang = u.HoTen,
                SoDienThoai = u.SoDienThoai,
                NgayTaoDon = trans.NgayTaoDon,
                TrangThai = trans.TrangThai,
                TongTien = trans.TongTien
            };
            return View(w);
        }
        public async Task<IActionResult> WaittingConfirmed(TransView transview)
        {
            var w = new Transaction
            {
                TransID = transview.TransID,
                KhachHangID = transview.KhachHangID,
                ThanhToan = transview.ThanhToan,
                NgayTaoDon = (DateTime)transview.NgayTaoDon,
                TrangThai = "Chờ duyệt",
                NguoiXuLyID = _context.User.Where(u => u.Username.Equals(transview.HoTenNguoiXuLy)).Select(u => u.UserID).FirstOrDefault(),
                ChietKhau = transview.ChietKhau,
                TongTien = transview.TongTien
            };
            _context.Transaction.Update(w);
            await _context.SaveChangesAsync();
            TempData["alertMessage"] = "Xác nhận đơn hàng thành công!";
            return RedirectToAction("Waitting", "TransactionManage");
        }
        public IActionResult Pending()
        {
            var w = (from trans in _context.Transaction
                     join u in _context.User on trans.KhachHangID equals u.UserID
                     where trans.TrangThai == "Chờ duyệt"
                     select new TransView
                     {
                         TransID = trans.TransID,
                         KhachHangID = trans.KhachHangID,
                         HoTenKhachHang = u.HoTen,
                         SoDienThoai = u.SoDienThoai,
                         ThanhToan = trans.ThanhToan,
                         NgayTaoDon = trans.NgayTaoDon,
                         TrangThai = trans.TrangThai,
                         NguoiXuLyID = trans.NguoiXuLyID,
                         HoTenNguoiXuLy = _context.User.Where(u => u.UserID.Equals(trans.NguoiXuLyID)).Select(u => u.HoTen).FirstOrDefault(),
                         TongTien = trans.TongTien
                     }).ToList();
            return View(w);
        }
        public IActionResult PendingConfirm(long transid)
        {
            var trans = _context.Transaction.Where(t => t.TransID.Equals(transid)).FirstOrDefault();
            var u = _context.User.Where(u => u.UserID.Equals(trans.KhachHangID)).FirstOrDefault();
            var w = new TransView
            {
                TransID = trans.TransID,
                KhachHangID = trans.KhachHangID,
                HoTenKhachHang = u.HoTen,
                SoDienThoai = u.SoDienThoai,
                ThanhToan = trans.ThanhToan,
                NgayTaoDon = trans.NgayTaoDon,
                TrangThai = trans.TrangThai,
                NguoiXuLyID = trans.NguoiXuLyID,
                HoTenNguoiXuLy = _context.User.Where(u => u.UserID.Equals(trans.NguoiXuLyID)).Select(u => u.HoTen).FirstOrDefault(),
                TongTien = trans.TongTien
            };
            return View(w);
        }
        public async Task<IActionResult> PendingConfirmed(TransView transview)
        {
            var trans = new Transaction
            {
                TransID = transview.TransID,
                KhachHangID = transview.KhachHangID,
                NgayTaoDon = (DateTime)transview.NgayTaoDon,
                ThanhToan = transview.ThanhToan,
                TrangThai = "Hoàn thành",
                NguoiXuLyID = transview.NguoiXuLyID,
                NgayDuyet = DateTime.Now,
                NguoiDuyetID = _context.User.Where(u => u.Username.Equals(transview.HoTenNguoiDuyet)).Select(u => u.UserID).FirstOrDefault(),
                ChietKhau = transview.ChietKhau,
                TongTien = transview.TongTien
            };
            _context.Transaction.Update(trans);
            await _context.SaveChangesAsync();


            //var carInTrans = _context.Car.Where(c => c.TransactionID.Equals(trans.TransID)).ToList();
            //foreach (var car in carInTrans)
            //{
            //    car.
            //    _context.Car.Add(car);
            //    await _context.SaveChangesAsync();
            //}

            TempData["alertMessage"] = "Duyệt đơn hàng thành công!";
            return RedirectToAction("Pending", "TransactionManage");
        }
        public IActionResult Approved()
        {
            var w = (from trans in _context.Transaction
                     join u in _context.User on trans.KhachHangID equals u.UserID
                     where trans.TrangThai == "Hoàn thành"
                     select new TransView
                     {
                         TransID = trans.TransID,
                         KhachHangID = trans.KhachHangID,
                         HoTenKhachHang = _context.User.Where(u => u.UserID.Equals(trans.KhachHangID)).Select(u => u.HoTen).FirstOrDefault(),
                         SoDienThoai = u.SoDienThoai,
                         NgayTaoDon = trans.NgayTaoDon,
                         ThanhToan = trans.ThanhToan,
                         TrangThai = trans.TrangThai,
                         NguoiXuLyID = trans.NguoiXuLyID,
                         HoTenNguoiXuLy = _context.User.Where(u => u.UserID.Equals(trans.NguoiXuLyID)).Select(u => u.HoTen).FirstOrDefault(),
                         NgayDuyet = trans.NgayDuyet,
                         NguoiDuyetID = trans.NguoiDuyetID,
                         HoTenNguoiDuyet = _context.User.Where(u => u.UserID.Equals(trans.NguoiDuyetID)).Select(u => u.HoTen).FirstOrDefault(),
                         ChietKhau = trans.ChietKhau,
                         TongTien = trans.TongTien
                     }).ToList();
            return View(w);
        }
        public async Task<IActionResult> Cancel(long transid)
        {
            var cancelt = _context.Transaction.Find(transid);
            var action = "";
            if (cancelt.NguoiXuLyID == null)
            {
                action = "Waitting";
            }
            else
            {
                action = "Pending";
            }
            var canceltran = new Transaction
            {
                TransID = transid,
                KhachHangID = cancelt.KhachHangID,
                NgayTaoDon = cancelt.NgayTaoDon,
                ThanhToan = cancelt.ThanhToan,
                TrangThai = "Đã hủy",
                NguoiXuLyID = cancelt.NguoiXuLyID,
                NgayDuyet = cancelt.NgayDuyet,
                NguoiDuyetID = cancelt.NguoiDuyetID,
                ChietKhau = cancelt.ChietKhau,
                TongTien = cancelt.TongTien
            };
            _context.Transaction.Update(canceltran);
            await _context.SaveChangesAsync();
            
            var carInTrans = _context.Car.Where(c => c.TransactionID.Equals(canceltran.TransID)).ToList();
            foreach (var car in carInTrans)
            {
                car.TransactionID = null;
                _context.Car.Update(car);
                await _context.SaveChangesAsync();
            }

            TempData["alertMessage"] = "Đã hủy đơn hàng!";
            return RedirectToAction(action, "TransactionManage");
        }
        public IActionResult Cancelled()
        {
            var canceltransview = (from trans in _context.Transaction
                                   join u in _context.User on trans.KhachHangID equals u.UserID
                                   where trans.TrangThai == "Đã hủy"
                                   select new TransView
                                   {
                                       TransID = trans.TransID,
                                       KhachHangID = trans.KhachHangID,
                                       HoTenKhachHang = _context.User.Where(u => u.UserID.Equals(trans.KhachHangID)).Select(u => u.HoTen).FirstOrDefault(),
                                       SoDienThoai = u.SoDienThoai,
                                       NgayTaoDon = trans.NgayTaoDon,
                                       ThanhToan = trans.ThanhToan,
                                       TrangThai = trans.TrangThai,
                                       NguoiXuLyID = trans.NguoiXuLyID,
                                       HoTenNguoiXuLy = _context.User.Where(u => u.UserID.Equals(trans.NguoiXuLyID)).Select(u => u.HoTen).FirstOrDefault(),
                                       NgayDuyet = trans.NgayDuyet,
                                       NguoiDuyetID = trans.NguoiDuyetID,
                                       HoTenNguoiDuyet = _context.User.Where(u => u.UserID.Equals(trans.NguoiDuyetID)).Select(u => u.HoTen).FirstOrDefault(),
                                       ChietKhau = trans.ChietKhau,
                                       TongTien = trans.TongTien
                                   }).ToList();
            return View(canceltransview);
        }
    }
}
