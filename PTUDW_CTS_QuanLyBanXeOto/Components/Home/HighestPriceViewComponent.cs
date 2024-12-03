using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components.Home
{
    //Tạo viewcomponent xe giá cao nhất để hiển thị ở trang chủ
    [ViewComponent(Name = "HighestPrice")]
    public class HighestPriceViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public HighestPriceViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var HighestPrice = (from ctp in _context.CarType
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where ctp.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                orderby ctp.GiaBan descending
                                select new CarTypeModel
                                {
                                    TenHangXe = hx.TenHangXe,
                                    TenDongXe = dx.TenDongXe,
                                    DoiXe = ctp.DoiXe,
                                    MauSac = ctp.MauSac,
                                    DongCo = ctp.DongCo,
                                    GiaBan = ctp.GiaBan,
                                    CarImage = ctp.CarImage
                                }).Take(5).ToList();

            foreach (var car in HighestPrice)
            {
                car.SoLuong = _context.Car.Where(c => c.TransactionID == null && c.CarTypeID.Equals(car.CarTypeID)).Count();
                if (!string.IsNullOrEmpty(car.CarImage))
                {
                    car.CarImage = car.CarImage.Split(',')[0]; // Lấy ảnh đầu tiên từ chuỗi CarImage
                }
            }

            return await Task.FromResult((IViewComponentResult)View("HighestPrice", HighestPrice));
        }
    }
}
