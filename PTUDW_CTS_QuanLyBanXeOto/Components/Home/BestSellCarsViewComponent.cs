using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components
{
    //Tạo viewcomponent hiển thị các user là Admin trong Trang chủ
    [ViewComponent(Name = "BestSellCars")]
    public class BestSellCarsViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public BestSellCarsViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofBestSellingCars = (from ctp in _context.CarType
                                         join c in _context.Car on ctp.CarTypeID equals c.CarTypeID
                                         join t in _context.Transaction on c.TransactionID equals t.TransID
                                         join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                         join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                         where t.TrangThai == "Hoàn thành"
                                         where ctp.IsDeleted == false && c.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                         group c by new { hx.HangXeID, hx.TenHangXe, dx.DongXeID, dx.TenDongXe, ctp.DoiXe, ctp.MauSac, ctp.DongCo, ctp.GiaBan, ctp.CarImage } into g
                                         orderby g.Count() descending
                                         select new CarTypeModel
                                         {
                                             HangXeID = g.Key.HangXeID,
                                             TenHangXe = g.Key.TenHangXe,
                                             DongXeID = g.Key.DongXeID,
                                             TenDongXe = g.Key.TenDongXe,
                                             DoiXe = g.Key.DoiXe,
                                             MauSac = g.Key.MauSac,
                                             DongCo = g.Key.DongCo,
                                             GiaBan = g.Key.GiaBan,
                                             CarImage = g.Key.CarImage
                                         }).Take(5).ToList();

            foreach (var car in listofBestSellingCars)
            {
                car.SoLuong = _context.Car.Where(c => c.TransactionID == null && c.CarTypeID.Equals(car.CarTypeID)).Count();
                if (!string.IsNullOrEmpty(car.CarImage))
                {
                    car.CarImage = car.CarImage.Split(',')[0]; // Lấy ảnh đầu tiên từ chuỗi CarImage
                }
            }

            return await Task.FromResult((IViewComponentResult)View("BestSellCars", listofBestSellingCars));
        }
    }
}
