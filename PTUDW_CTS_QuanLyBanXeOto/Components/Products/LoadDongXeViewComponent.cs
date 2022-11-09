using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components
{
    //Tạo viewcomponent để trả về dòng xe với hãng xe được chọn
    [ViewComponent(Name = "LoadDongXe")]
    public class LoadDongXeViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public LoadDongXeViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string TenHangXe)
        {
            //Lấy dữ liệu từ sql
            var listofDongXe = (from hx in _context.HangXe
                                join dx in _context.DongXe                                
                                on hx.HangXeID equals dx.HangXeID
                                join c in _context.Car
                                on dx.DongXeID equals c.DongXeID
                                where hx.TenHangXe == TenHangXe
                                select new
                                {
                                    TenHangXe = TenHangXe,
                                    TenDongXe = dx.TenDongXe,
                                    DoiXe = c.DoiXe,
                                    MauSac = c.MauSac,
                                    DongCo = c.DongCo,
                                    GiaBan = c.GiaBan,
                                    CarImage = c.CarImage
                                }).AsEnumerable().Select(c => new Category
                                {
                                    TenHangXe = c.TenHangXe,
                                    TenDongXe = c.TenDongXe,
                                    DoiXe = c.DoiXe,
                                    MauSac = c.MauSac,
                                    DongCo = c.DongCo,
                                    GiaBan = c.GiaBan,
                                    CarImage = c.CarImage
                                }).ToList();
            //Trả về view default với dữ liệu listofDongXe là những dòng xe thuộc hãng xe mình chọn
            return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
        }
    }
}