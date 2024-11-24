using Microsoft.AspNetCore.Mvc;
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
            var listofBestSellingCars = (from ct in _context.CarTrans
                                         join t in _context.Transaction on ct.TransID equals t.TransID
                                         join c in _context.Car on ct.CarID equals c.CarID
                                         join dx in _context.DongXe on c.DongXeID equals dx.DongXeID
                                         join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                         where t.TrangThai == "Thành công"
                                         group ct by new { c.CarID, hx.TenHangXe, dx.TenDongXe, c.DoiXe, c.MauSac, c.DongCo, c.GiaBan, c.CarImage } into g
                                         orderby g.Count() descending
                                         select new CarModel
                                         {
                                             TenHangXe = g.Key.TenHangXe,
                                             TenDongXe = g.Key.TenDongXe,
                                             DoiXe = g.Key.DoiXe,
                                             MauSac = g.Key.MauSac,
                                             DongCo = g.Key.DongCo,
                                             GiaBan = g.Key.GiaBan,
                                             CarImage = g.Key.CarImage,
                                             SoLuong = g.Count() // Số lượng bán
                                         }).Take(5).ToList();

            foreach (var car in listofBestSellingCars)
            {
                if (!string.IsNullOrEmpty(car.CarImage))
                {
                    car.CarImage = car.CarImage.Split(',')[0]; // Lấy ảnh đầu tiên từ chuỗi CarImage
                }
            }

            //Trả về view NewestCar với dữ liệu là listofNewestCar là những chiếc xe vừa được nhập mới nhất
            return await Task.FromResult((IViewComponentResult)View("BestSellCars", listofBestSellingCars));
        }
    }
}
