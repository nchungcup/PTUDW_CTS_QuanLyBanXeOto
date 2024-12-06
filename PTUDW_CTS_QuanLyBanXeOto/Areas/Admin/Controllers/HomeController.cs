using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers
{
    //Tạo controller cho phần home của Admin
    [Area("Admin")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext) : base()
        {
            _logger = logger;
            _context = dataContext;
        }

        //Method trả về view Index của Admin
        [Route(nameof(Admin) + "/Home")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetTransactionCount(int type)
        {
            var transCountCurrent = 0;
            if (type == 1)
            {
                var currentDay = DateTime.Today.Day;
                var currentMonth = DateTime.Today.Month;
                var currentYear = DateTime.Today.Year;

                transCountCurrent = _context.Transaction
                    .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                t.NgayDuyet.Value.Day == currentDay &&
                                t.NgayDuyet.Value.Month == currentMonth &&
                                t.NgayDuyet.Value.Year == currentYear)
                    .Count();
            }
            else if (type == 2)
            {
                var currentMonth = DateTime.Today.Month;
                var currentYear = DateTime.Today.Year;

                transCountCurrent = _context.Transaction
                    .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                t.NgayDuyet.Value.Month == currentMonth &&
                                t.NgayDuyet.Value.Year == currentYear)
                    .Count();

            }
            else if (type == 3)
            {
                var currentYear = DateTime.Today.Year;

                transCountCurrent = _context.Transaction
                    .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                t.NgayDuyet.Value.Year == currentYear)
                    .Count();

            }
            return Json(new { count = transCountCurrent });
        }

        public IActionResult GetProfit(int type)
        {
            long profitCurrent = 0;
            if (type == 1)
            {
                var currentDay = DateTime.Today.Day;
                var currentMonth = DateTime.Today.Month;
                var currentYear = DateTime.Today.Year;

                var trans = _context.Transaction
                    .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                t.NgayDuyet.Value.Day == currentDay &&
                                t.NgayDuyet.Value.Month == currentMonth &&
                                t.NgayDuyet.Value.Year == currentYear);
                var tranIds = trans.Select(t => t.TransID).ToList();
                var giaNhap = _context.Car.Where(c => tranIds.Contains(c.TransactionID.GetValueOrDefault())).Sum(c => c.GiaNhap);
                var giaBan = trans.Sum(t => t.TongTien);
                profitCurrent = (long)(giaBan - giaNhap);

            }
            else if (type == 2)
            {
                var currentMonth = DateTime.Today.Month;
                var currentYear = DateTime.Today.Year;

                var trans = _context.Transaction
                    .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                t.NgayDuyet.Value.Month == currentMonth &&
                                t.NgayDuyet.Value.Year == currentYear);
                var tranIds = trans.Select(t => t.TransID).ToList();
                var giaNhap = _context.Car.Where(c => tranIds.Contains(c.TransactionID.GetValueOrDefault())).Sum(c => c.GiaNhap);
                var giaBan = trans.Sum(t => t.TongTien);
                profitCurrent = (long)(giaBan - giaNhap);
            }
            else if (type == 3)
            {
                var currentYear = DateTime.Today.Year;

                var trans = _context.Transaction
                    .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                t.NgayDuyet.Value.Year == currentYear);
                var tranIds = trans.Select(t => t.TransID).ToList();
                var giaNhap = _context.Car.Where(c => tranIds.Contains(c.TransactionID.GetValueOrDefault())).Sum(c => c.GiaNhap);
                var giaBan = trans.Sum(t => t.TongTien);
                profitCurrent = (long)(giaBan - giaNhap);

            }
            return Json(new { profit = profitCurrent });
        }

        public IActionResult GetCustomerCount()
        {
            var cusCount = _context.User.Where(u => u.TypeID.Equals(2) && u.IsActive.Equals(true)).Count();
            return Json(new { cusCount = cusCount });
        }

        public IActionResult GetTransactionAndProfitForChart(int type)
        {
            var model = new HomeReportModel
            {
                Time = new List<string>(), // Khởi tạo danh sách thời gian
                TransactionCount = new List<long>(), // Khởi tạo danh sách số lượng đơn hàng
                Profit = new List<long>() // Khởi tạo danh sách lợi nhuận
            };

            if (type == 1) // Theo giờ
            {
                var currentDay = DateTime.Today;
                var startOfDay = currentDay.Date; // Bắt đầu từ đầu ngày hôm nay

                for (int hour = 0; hour < 24; hour++) // Lặp qua 24 giờ trong ngày
                {
                    var hourStart = startOfDay.AddHours(hour);
                    var hourEnd = hourStart.AddHours(1).AddTicks(-1); // Kết thúc giờ

                    // Lấy các giao dịch "Hoàn thành" trong khoảng giờ này
                    var trans = _context.Transaction
                        .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                    t.NgayDuyet.HasValue &&
                                    t.NgayDuyet.Value >= hourStart &&
                                    t.NgayDuyet.Value <= hourEnd)
                        .ToList();

                    // Lấy lợi nhuận trong giờ này
                    var tranIds = trans.Select(t => t.TransID).ToList();
                    var giaNhap = _context.Car.Where(c => tranIds.Contains(c.TransactionID.GetValueOrDefault())).Sum(c => c.GiaNhap);
                    var giaBan = trans.Sum(t => t.TongTien);
                    var profit = giaBan - giaNhap;

                    model.Time.Add(hourStart.ToString("HH:00")); // Thêm thời gian vào danh sách
                    model.TransactionCount.Add(trans.Count); // Thêm số lượng đơn hàng vào danh sách
                    model.Profit.Add((long)profit); // Thêm lợi nhuận vào danh sách
                }
            }
            else if (type == 2) // Theo ngày
            {
                var currentMonth = DateTime.Today.Month;
                var currentYear = DateTime.Today.Year;

                // Lặp qua tất cả các ngày trong tháng hiện tại
                for (int day = 1; day <= DateTime.DaysInMonth(currentYear, currentMonth); day++)
                {
                    var dayStart = new DateTime(currentYear, currentMonth, day);
                    var dayEnd = dayStart.AddDays(1).AddTicks(-1); // Kết thúc ngày

                    // Lấy các giao dịch "Hoàn thành" trong ngày này
                    var trans = _context.Transaction
                        .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                    t.NgayDuyet.HasValue &&
                                    t.NgayDuyet.Value.Date == dayStart.Date)
                        .ToList();

                    // Lấy lợi nhuận trong ngày này
                    var tranIds = trans.Select(t => t.TransID).ToList();
                    var giaNhap = _context.Car.Where(c => tranIds.Contains(c.TransactionID.GetValueOrDefault())).Sum(c => c.GiaNhap);
                    var giaBan = trans.Sum(t => t.TongTien);
                    var profit = giaBan - giaNhap;

                    model.Time.Add(dayStart.ToString("yyyy-MM-dd")); // Thêm thời gian vào danh sách (ngày)
                    model.TransactionCount.Add(trans.Count); // Thêm số lượng đơn hàng vào danh sách
                    model.Profit.Add((long)profit); // Thêm lợi nhuận vào danh sách
                }
            }
            else if (type == 3) // Theo tháng
            {
                var currentYear = DateTime.Today.Year;

                // Lặp qua tất cả các tháng trong năm hiện tại
                for (int month = 1; month <= 12; month++)
                {
                    var monthStart = new DateTime(currentYear, month, 1);
                    var monthEnd = monthStart.AddMonths(1).AddTicks(-1); // Kết thúc tháng

                    // Lấy các giao dịch "Hoàn thành" trong tháng này
                    var trans = _context.Transaction
                        .Where(t => t.TrangThai.Equals("Hoàn thành") &&
                                    t.NgayDuyet.HasValue &&
                                    t.NgayDuyet.Value.Month == month &&
                                    t.NgayDuyet.Value.Year == currentYear)
                        .ToList();

                    // Lấy lợi nhuận trong tháng này
                    var tranIds = trans.Select(t => t.TransID).ToList();
                    var giaNhap = _context.Car.Where(c => tranIds.Contains(c.TransactionID.GetValueOrDefault())).Sum(c => c.GiaNhap);
                    var giaBan = trans.Sum(t => t.TongTien);
                    var profit = giaBan - giaNhap;

                    model.Time.Add(monthStart.ToString("yyyy-MM")); // Thêm thời gian vào danh sách (tháng)
                    model.TransactionCount.Add(trans.Count); // Thêm số lượng đơn hàng vào danh sách
                    model.Profit.Add((long)profit); // Thêm lợi nhuận vào danh sách
                }
            }

            return Json(model); // Trả về đối tượng model
        }

    }
}
