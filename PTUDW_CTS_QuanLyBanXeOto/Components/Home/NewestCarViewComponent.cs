using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components
{
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
                var listofNewestCar = (from c in _context.Car
                                       join dx in _context.DongXe
                                       on c.DongXeID equals dx.DongXeID
                                       join hx in _context.HangXe
                                       on dx.HangXeID equals hx.HangXeID
                                        select new
                                        {
                                        TenHangXe = hx.TenHangXe,
                                        TenDongXe = dx.TenDongXe,
                                        DoiXe = c.DoiXe,
                                        MauSac = c.MauSac,
                                        DongCo = c.DongCo,
                                        GiaBan = c.GiaBan,
                                        CarImage = c.CarImage
                                        }).Take(2).AsEnumerable().Select(cate => new Category
                                        {
                                        TenHangXe = cate.TenHangXe,
                                        TenDongXe = cate.TenDongXe,
                                        DoiXe = cate.DoiXe,
                                        MauSac = cate.MauSac,
                                        DongCo = cate.DongCo,
                                        GiaBan = cate.GiaBan,
                                        CarImage = cate.CarImage
                                    }).OrderByDescending(cate => cate.NgayNhap).ToList();
                return await Task.FromResult((IViewComponentResult)View("NewestCar", listofNewestCar));
            }
        }
}
