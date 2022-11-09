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
                var listofNewestCar = (from c in _context.Car
                                       join dx in _context.DongXe
                                       on c.DongXeID equals dx.DongXeID
                                       join hx in _context.HangXe
                                       on dx.HangXeID equals hx.HangXeID
                                       orderby c.NgayNhap descending
                                        select new
                                        {
                                        TenHangXe = hx.TenHangXe,
                                        TenDongXe = dx.TenDongXe,
                                        DoiXe = c.DoiXe,
                                        MauSac = c.MauSac,
                                        DongCo = c.DongCo,
                                        GiaBan = c.GiaBan,
                                        CarImage = c.CarImage,
                                        NgayNhap = c.NgayNhap
                                        }).Take(2).AsEnumerable().Select(cate => new Category
                                        {
                                        TenHangXe = cate.TenHangXe,
                                        TenDongXe = cate.TenDongXe,
                                        DoiXe = cate.DoiXe,
                                        MauSac = cate.MauSac,
                                        DongCo = cate.DongCo,
                                        GiaBan = cate.GiaBan,
                                        CarImage = cate.CarImage,
                                        NgayNhap = cate.NgayNhap
                                    }).ToList();
            //Trả về view NewestCar với dữ liệu là listofNewestCar là những chiếc xe vừa được nhập mới nhất
                return await Task.FromResult((IViewComponentResult)View("NewestCar", listofNewestCar));
            }
        }
}
