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
                     where trans.TrangThai == "Waitting"
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
                         Thue = trans.Thue,
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
                NgayTaoDon = transview.NgayTaoDon,
                TrangThai = "Pending",
                NguoiXuLyID = _context.User.Where(u => u.Username.Equals(transview.HoTenNguoiXuLy)).Select(u => u.UserID).FirstOrDefault(),
                ChietKhau = transview.ChietKhau,
                Thue = transview.Thue,
                TongTien = transview.TongTien
            };
            _context.Transaction.Update(w);
            await _context.SaveChangesAsync();
            TempData["alertMessage"] = "Action Completed";
            return RedirectToAction("Waitting", "TransactionManage");
        }
        public IActionResult Pending()
        {
            var w = (from trans in _context.Transaction
                     join u in _context.User on trans.KhachHangID equals u.UserID
                     where trans.TrangThai == "Pending"
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
                         Thue = trans.Thue,
                         TongTien = trans.TongTien
                     };
            return View(w);
        }
        public async Task<IActionResult> PendingConfirmed(TransView transview)
        {
            var w = new Transaction
            {
                TransID = transview.TransID,
                KhachHangID = transview.KhachHangID,
                NgayTaoDon = transview.NgayTaoDon,
                ThanhToan = transview.ThanhToan,
                TrangThai = "Approved",
                NguoiXuLyID = transview.NguoiXuLyID,
                NgayDuyet = DateTime.Now,
                NguoiDuyetID = _context.User.Where(u => u.Username.Equals(transview.HoTenNguoiDuyet)).Select(u => u.UserID).FirstOrDefault(),
                ChietKhau = transview.ChietKhau,
                Thue = transview.Thue,
                TongTien = transview.TongTien
            };
            _context.Transaction.Update(w);
            await _context.SaveChangesAsync();
            TempData["alertMessage"] = "Action Completed!";
            return RedirectToAction("Pending", "TransactionManage");
        }
        public IActionResult Approved()
        {
            var w = (from trans in _context.Transaction
                     join u in _context.User on trans.KhachHangID equals u.UserID
                     where trans.TrangThai == "Approved"
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
                         Thue = trans.Thue,
                         TongTien = trans.TongTien
                     }).ToList();
            return View(w);
        }
        public async Task<IActionResult> Cancel(long transid)
        {
            var cancelt = _context.Transaction.Find(transid);
            var cancelct = _context.CarTrans.Where(ct => ct.TransID.Equals(transid)).Select(ct => ct.CarID).ToList();
            foreach(var ct in cancelct)
            {
                var updatect = new Car_Trans
                {
                    CarID = ct,
                    TransID = null
                };
                _context.CarTrans.Update(updatect);
                await _context.SaveChangesAsync();
            }
            var canceltran = new Transaction
            {
                TransID = transid,
                KhachHangID = cancelt.KhachHangID,
                NgayTaoDon = cancelt.NgayTaoDon,
                ThanhToan = cancelt.ThanhToan,
                TrangThai = "Cancelled",
                NguoiXuLyID = cancelt.NguoiXuLyID,
                NgayDuyet = cancelt.NgayDuyet,
                NguoiDuyetID = cancelt.NguoiDuyetID,
                ChietKhau = cancelt.ChietKhau,
                Thue = cancelt.Thue,
                TongTien = cancelt.TongTien
            };
            _context.Transaction.Update(canceltran);
            await _context.SaveChangesAsync();
            return RedirectToAction("Cancelled", "TransactionManage");
        }
        public IActionResult Cancelled()
        {
            var canceltransview = (from trans in _context.Transaction
                                   join u in _context.User on trans.KhachHangID equals u.UserID
                                   where trans.TrangThai == "Cancelled"
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
                                       Thue = trans.Thue,
                                       TongTien = trans.TongTien
                                   }).ToList();
            return View(canceltransview);
        }
    }
}
