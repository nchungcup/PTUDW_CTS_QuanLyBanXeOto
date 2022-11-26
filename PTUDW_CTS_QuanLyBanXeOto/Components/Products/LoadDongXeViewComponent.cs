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
        public async Task<IViewComponentResult> InvokeAsync(string TenHangXe, string MauSac, string DongCo)
        {
            if (TenHangXe == "All" && DongCo == null && MauSac == null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                //Lấy dữ liệu từ sql
                var listofDongXe = (from dx in _context.DongXe
                                    join c in _context.Car on dx.DongXeID equals c.DongXeID
                                    join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                    from ct in ctc.DefaultIfEmpty()
                                    where ct.TransID == null
                                    group c by new { c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                    select new Category
                                    {
                                        TenHangXe = TenHangXe,
                                        DongXeID = groupc.Key.DongXeID,
                                        TenDongXe = groupc.Key.TenDongXe,
                                        DoiXe = groupc.Key.DoiXe,
                                        MauSac = groupc.Key.MauSac,
                                        DongCo = groupc.Key.DongCo,
                                        GiaBan = groupc.Key.GiaBan,
                                        CarImage = groupc.Key.CarImage,
                                        SoLuong = groupc.Count()
                                    }).ToList();
                //Trả về view default với dữ liệu listofDongXe là những dòng xe thuộc hãng xe mình chọn
                return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
            }
            else if (TenHangXe == "All" && DongCo != null && MauSac == null)
            {
                var listofDongXe = (from dx in _context.DongXe
                                    join c in _context.Car on dx.DongXeID equals c.DongXeID
                                    join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                    from ct in ctc.DefaultIfEmpty()
                                    where c.DongCo == DongCo && ct.TransID == null
                                    group c by new { c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                    select new Category
                                    {
                                        TenHangXe = TenHangXe,
                                        DongXeID = groupc.Key.DongXeID,
                                        TenDongXe = groupc.Key.TenDongXe,
                                        DoiXe = groupc.Key.DoiXe,
                                        MauSac = groupc.Key.MauSac,
                                        DongCo = groupc.Key.DongCo,
                                        GiaBan = groupc.Key.GiaBan,
                                        CarImage = groupc.Key.CarImage,
                                        SoLuong = groupc.Count()
                                    }).ToList();
                //Trả về view default với dữ liệu listofDongXe là những dòng xe thuộc hãng xe mình chọn
                return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
            }
            else if (TenHangXe == "All" && DongCo == null && MauSac != null)
            {
                var listofDongXe = (from dx in _context.DongXe
                                    join c in _context.Car on dx.DongXeID equals c.DongXeID
                                    join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                    from ct in ctc.DefaultIfEmpty()
                                    where c.MauSac == MauSac && ct.TransID == null
                                    group c by new { c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                    select new Category
                                    {
                                        TenHangXe = TenHangXe,
                                        DongXeID = groupc.Key.DongXeID,
                                        TenDongXe = groupc.Key.TenDongXe,
                                        DoiXe = groupc.Key.DoiXe,
                                        MauSac = groupc.Key.MauSac,
                                        DongCo = groupc.Key.DongCo,
                                        GiaBan = groupc.Key.GiaBan,
                                        CarImage = groupc.Key.CarImage,
                                        SoLuong = groupc.Count()
                                    }).ToList();
                //Trả về view default với dữ liệu listofDongXe là những dòng xe thuộc hãng xe mình chọn
                return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
            }
            else if(TenHangXe != null && MauSac == null && DongCo == null)
            {     
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                //Lấy dữ liệu từ sql
                var listofDongXe = (from dx in _context.DongXe
                                    join c in _context.Car on dx.DongXeID equals c.DongXeID
                                    join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                    from ct in ctc.DefaultIfEmpty()
                                    where dx.HangXeID == hxid && ct.TransID == null
                                    group c by new { c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                    select new Category
                                    {
                                        TenHangXe = TenHangXe,
                                        DongXeID = groupc.Key.DongXeID,
                                        TenDongXe = groupc.Key.TenDongXe,
                                        DoiXe = groupc.Key.DoiXe,
                                        MauSac = groupc.Key.MauSac,
                                        DongCo = groupc.Key.DongCo,
                                        GiaBan = groupc.Key.GiaBan,
                                        CarImage = groupc.Key.CarImage,
                                        SoLuong = groupc.Count()
                                    }).ToList();
                //Trả về view default với dữ liệu listofDongXe là những dòng xe thuộc hãng xe mình chọn
                return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
            }
            else if (TenHangXe != null && DongCo != null && MauSac == null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                var listofDongXe = (from dx in _context.DongXe
                                    join c in _context.Car on dx.DongXeID equals c.DongXeID
                                    join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                    from ct in ctc.DefaultIfEmpty()
                                    where dx.HangXeID == hxid && c.DongCo == DongCo && ct.TransID == null
                                    group c by new { c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                    select new Category
                                    {
                                        TenHangXe = TenHangXe,
                                        DongXeID = groupc.Key.DongXeID,
                                        TenDongXe = groupc.Key.TenDongXe,
                                        DoiXe = groupc.Key.DoiXe,
                                        MauSac = groupc.Key.MauSac,
                                        DongCo = groupc.Key.DongCo,
                                        GiaBan = groupc.Key.GiaBan,
                                        CarImage = groupc.Key.CarImage,
                                        SoLuong = groupc.Count()
                                    }).ToList();
                //Trả về view default với dữ liệu listofDongXe là những dòng xe thuộc hãng xe mình chọn
                return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
            }
            else if (TenHangXe != null && DongCo == null && MauSac != null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                var listofDongXe = (from dx in _context.DongXe
                                    join c in _context.Car on dx.DongXeID equals c.DongXeID
                                    join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                    from ct in ctc.DefaultIfEmpty()
                                    where dx.HangXeID == hxid && c.MauSac == MauSac && ct.TransID == null
                                    group c by new { c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                    select new Category
                                    {
                                        TenHangXe = TenHangXe,
                                        DongXeID = groupc.Key.DongXeID,
                                        TenDongXe = groupc.Key.TenDongXe,
                                        DoiXe = groupc.Key.DoiXe,
                                        MauSac = groupc.Key.MauSac,
                                        DongCo = groupc.Key.DongCo,
                                        GiaBan = groupc.Key.GiaBan,
                                        CarImage = groupc.Key.CarImage,
                                        SoLuong = groupc.Count()
                                    }).ToList();
                //Trả về view default với dữ liệu listofDongXe là những dòng xe thuộc hãng xe mình chọn
                return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
            }
            else
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                var listofDongXe = (from dx in _context.DongXe
                                    join c in _context.Car on dx.DongXeID equals c.DongXeID
                                    join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                    from ct in ctc.DefaultIfEmpty()
                                    where dx.HangXeID == hxid && c.MauSac == MauSac && c.DongCo == DongCo && ct.TransID == null
                                    group c by new { c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                    select new Category
                                    {
                                        TenHangXe = TenHangXe,
                                        DongXeID = groupc.Key.DongXeID,
                                        TenDongXe = groupc.Key.TenDongXe,
                                        DoiXe = groupc.Key.DoiXe,
                                        MauSac = groupc.Key.MauSac,
                                        DongCo = groupc.Key.DongCo,
                                        GiaBan = groupc.Key.GiaBan,
                                        CarImage = groupc.Key.CarImage,
                                        SoLuong = groupc.Count()
                                    }).ToList();
                //Trả về view default với dữ liệu listofDongXe là những dòng xe thuộc hãng xe mình chọn
                return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
            }
        }
    }
}