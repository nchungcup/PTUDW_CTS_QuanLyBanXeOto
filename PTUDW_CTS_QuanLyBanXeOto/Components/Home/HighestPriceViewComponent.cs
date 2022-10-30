using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components.Home
{
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
            var HighestPrice = (from c in _context.Car
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
                                   }).Take(1).AsEnumerable().Select(cate => new Category
                                   {
                                       TenHangXe = cate.TenHangXe,
                                       TenDongXe = cate.TenDongXe,
                                       DoiXe = cate.DoiXe,
                                       MauSac = cate.MauSac,
                                       DongCo = cate.DongCo,
                                       GiaBan = cate.GiaBan,
                                       CarImage = cate.CarImage
                                   }).OrderBy(cate => cate.GiaBan).ToList();
            return await Task.FromResult((IViewComponentResult)View("HighestPrice", HighestPrice));
        }
    }
}
