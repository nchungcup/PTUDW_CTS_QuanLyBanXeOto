using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components
{
    //Tạo viewcomponent để hiển thị xe mới nhất vừa nhập trong sql
    [ViewComponent(Name = "NewestCar")]
    public class NewestCarViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public NewestCarViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Lấy dữ liệu từ sql
            var listofNewestCar = (from ctp in _context.CarType
                                   join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                   join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                   join c in _context.Car on ctp.CarTypeID equals c.CarTypeID
                                   where ctp.IsDeleted == false && c.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                   orderby c.CarID descending // Sắp xếp theo CarID giảm dần
                                   select new CarTypeModel
                                   {
                                       CarTypeID = ctp.CarTypeID,
                                       TenHangXe = hx.TenHangXe,
                                       TenDongXe = dx.TenDongXe,
                                       DoiXe = ctp.DoiXe,
                                       MauSac = ctp.MauSac,
                                       DongCo = ctp.DongCo,
                                       GiaBan = ctp.GiaBan,
                                       CarImage = ctp.CarImage
                                   }).Distinct() // Đảm bảo không bị trùng CarType
                       .Take(5) // Lấy 5 bản ghi đầu tiên
                       .ToList();

            foreach (var car in listofNewestCar)
            {
                car.SoLuong = _context.Car.Where(c => c.TransactionID == null && c.CarTypeID.Equals(car.CarTypeID)).Count();
                if (!string.IsNullOrEmpty(car.CarImage))
                {
                    car.CarImage = car.CarImage.Split(',')[0]; // Lấy ảnh đầu tiên từ chuỗi CarImage
                }
            }

            //Trả về view NewestCar với dữ liệu là listofNewestCar là những chiếc xe vừa được nhập mới nhất
            return await Task.FromResult((IViewComponentResult)View("NewestCar", listofNewestCar));
        }
    }
}
