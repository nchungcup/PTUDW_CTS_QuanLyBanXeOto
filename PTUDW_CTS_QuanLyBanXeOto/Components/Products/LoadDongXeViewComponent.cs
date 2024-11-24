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
            List<CarModel> listofDongXe = new List<CarModel>();
            if (TenHangXe == "All" && DongCo == null && MauSac == null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                //Lấy dữ liệu từ sql
                listofDongXe = (from dx in _context.DongXe
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                join c in _context.Car on dx.DongXeID equals c.DongXeID
                                join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                from ct in ctc.DefaultIfEmpty()
                                where ct.TransID == null
                                group c by new { hx.HangXeID, hx.TenHangXe, c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                select new CarModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    MauSac = groupc.Key.MauSac,
                                    DongCo = groupc.Key.DongCo,
                                    GiaBan = groupc.Key.GiaBan,
                                    CarImage = groupc.Key.CarImage,
                                    SoLuong = groupc.Count()
                                }).ToList();
            }
            else if (TenHangXe == "All" && DongCo != null && MauSac == null)
            {
                listofDongXe = (from dx in _context.DongXe
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                join c in _context.Car on dx.DongXeID equals c.DongXeID
                                join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                from ct in ctc.DefaultIfEmpty()
                                where c.DongCo == DongCo && ct.TransID == null
                                group c by new { hx.HangXeID, hx.TenHangXe, c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                select new CarModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    MauSac = groupc.Key.MauSac,
                                    DongCo = groupc.Key.DongCo,
                                    GiaBan = groupc.Key.GiaBan,
                                    CarImage = groupc.Key.CarImage,
                                    SoLuong = groupc.Count()
                                }).ToList();
            }
            else if (TenHangXe == "All" && DongCo == null && MauSac != null)
            {
                listofDongXe = (from dx in _context.DongXe
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                join c in _context.Car on dx.DongXeID equals c.DongXeID
                                join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                from ct in ctc.DefaultIfEmpty()
                                where c.MauSac == MauSac && ct.TransID == null
                                group c by new { hx.HangXeID, hx.TenHangXe, c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                select new CarModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    MauSac = groupc.Key.MauSac,
                                    DongCo = groupc.Key.DongCo,
                                    GiaBan = groupc.Key.GiaBan,
                                    CarImage = groupc.Key.CarImage,
                                    SoLuong = groupc.Count()
                                }).ToList();
            }
            else if (TenHangXe != null && MauSac == null && DongCo == null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                //Lấy dữ liệu từ sql
                listofDongXe = (from dx in _context.DongXe
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                join c in _context.Car on dx.DongXeID equals c.DongXeID
                                join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                from ct in ctc.DefaultIfEmpty()
                                where dx.HangXeID == hxid && ct.TransID == null
                                group c by new { hx.HangXeID, hx.TenHangXe, c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                select new CarModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    MauSac = groupc.Key.MauSac,
                                    DongCo = groupc.Key.DongCo,
                                    GiaBan = groupc.Key.GiaBan,
                                    CarImage = groupc.Key.CarImage,
                                    SoLuong = groupc.Count()
                                }).ToList();
            }
            else if (TenHangXe != null && DongCo != null && MauSac == null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                listofDongXe = (from dx in _context.DongXe
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                join c in _context.Car on dx.DongXeID equals c.DongXeID
                                join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                from ct in ctc.DefaultIfEmpty()
                                where dx.HangXeID == hxid && c.DongCo == DongCo && ct.TransID == null
                                group c by new { hx.HangXeID, hx.TenHangXe, c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                select new CarModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    MauSac = groupc.Key.MauSac,
                                    DongCo = groupc.Key.DongCo,
                                    GiaBan = groupc.Key.GiaBan,
                                    CarImage = groupc.Key.CarImage,
                                    SoLuong = groupc.Count()
                                }).ToList();
            }
            else if (TenHangXe != null && DongCo == null && MauSac != null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                listofDongXe = (from dx in _context.DongXe
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                join c in _context.Car on dx.DongXeID equals c.DongXeID
                                join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                from ct in ctc.DefaultIfEmpty()
                                where dx.HangXeID == hxid && c.MauSac == MauSac && ct.TransID == null
                                group c by new { hx.HangXeID, hx.TenHangXe, c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                select new CarModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    MauSac = groupc.Key.MauSac,
                                    DongCo = groupc.Key.DongCo,
                                    GiaBan = groupc.Key.GiaBan,
                                    CarImage = groupc.Key.CarImage,
                                    SoLuong = groupc.Count()
                                }).ToList();
            }
            else
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                listofDongXe = (from dx in _context.DongXe
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                join c in _context.Car on dx.DongXeID equals c.DongXeID
                                join ct in _context.CarTrans on c.CarID equals ct.CarID into ctc
                                from ct in ctc.DefaultIfEmpty()
                                where dx.HangXeID == hxid && c.MauSac == MauSac && c.DongCo == DongCo && ct.TransID == null
                                group c by new { hx.HangXeID, hx.TenHangXe, c.DongXeID, dx.TenDongXe, c.MauSac, c.DoiXe, c.DongCo, c.GiaBan, c.CarImage } into groupc
                                select new CarModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    MauSac = groupc.Key.MauSac,
                                    DongCo = groupc.Key.DongCo,
                                    GiaBan = groupc.Key.GiaBan,
                                    CarImage = groupc.Key.CarImage,
                                    SoLuong = groupc.Count()
                                }).ToList();
            }
            foreach (var car in listofDongXe)
            {
                if (!string.IsNullOrEmpty(car.CarImage))
                {
                    car.CarImage = car.CarImage.Split(',')[0]; // Lấy ảnh đầu tiên từ chuỗi CarImage
                }
            }
            return await Task.FromResult((IViewComponentResult)View("Default", listofDongXe));
        }
    }
}