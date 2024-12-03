using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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
        public async Task<IViewComponentResult> InvokeAsync(string TenHangXe, string MauSac, string DongCo, int DoiXe, long giaMin, long giaMax)
        {
            List<CarTypeModel> listofDongXe = new List<CarTypeModel>();
            if (TenHangXe == "All" && DongCo == null && MauSac == null)
            {
                //Lấy dữ liệu từ sql
                listofDongXe = (from ctp in _context.CarType
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where ctp.GiaBan >= giaMin && ctp.GiaBan <= giaMax && ctp.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                group ctp by new { hx.HangXeID, hx.TenHangXe, ctp.DongXeID, dx.TenDongXe, ctp.DoiXe, ctp.DongCo } into groupc
                                select new CarTypeModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    CarImage = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).FirstOrDefault().CarImage,
                                    GiaMin = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Min(),
                                    GiaMax = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Max(),
                                    MauSacKhaDung = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.MauSac).Distinct().ToList()
                                }).ToList();
            }
            else if (TenHangXe == "All" && DongCo != null && MauSac == null)
            {
                listofDongXe = (from ctp in _context.CarType
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where ctp.DongCo == DongCo && ctp.GiaBan >= giaMin && ctp.GiaBan <= giaMax && ctp.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                group ctp by new { hx.HangXeID, hx.TenHangXe, ctp.DongXeID, dx.TenDongXe, ctp.DoiXe, ctp.DongCo } into groupc
                                select new CarTypeModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    CarImage = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).FirstOrDefault().CarImage,
                                    GiaMin = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Min(),
                                    GiaMax = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Max(),
                                    MauSacKhaDung = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.MauSac).Distinct().ToList()
                                }).ToList();
            }
            else if (TenHangXe == "All" && DongCo == null && MauSac != null)
            {
                listofDongXe = (from ctp in _context.CarType
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where ctp.MauSac == MauSac && ctp.GiaBan >= giaMin && ctp.GiaBan <= giaMax && ctp.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                group ctp by new { hx.HangXeID, hx.TenHangXe, ctp.DongXeID, dx.TenDongXe, ctp.DoiXe, ctp.DongCo } into groupc
                                select new CarTypeModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    CarImage = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).FirstOrDefault().CarImage,
                                    GiaMin = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Min(),
                                    GiaMax = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Max(),
                                    MauSacKhaDung = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.MauSac).Distinct().ToList()
                                }).ToList();
            }
            else if (TenHangXe != null && MauSac == null && DongCo == null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                //Lấy dữ liệu từ sql
                listofDongXe = (from ctp in _context.CarType
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where dx.HangXeID == hxid && ctp.GiaBan >= giaMin && ctp.GiaBan <= giaMax && ctp.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                group ctp by new { hx.HangXeID, hx.TenHangXe, ctp.DongXeID, dx.TenDongXe, ctp.DoiXe, ctp.DongCo } into groupc
                                select new CarTypeModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    CarImage = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).FirstOrDefault().CarImage,
                                    GiaMin = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Min(),
                                    GiaMax = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Max(),
                                    MauSacKhaDung = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.MauSac).Distinct().ToList()
                                }).ToList();
            }
            else if (TenHangXe != null && DongCo != null && MauSac == null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                listofDongXe = (from ctp in _context.CarType
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where dx.HangXeID == hxid && ctp.DongCo == DongCo && ctp.GiaBan >= giaMin && ctp.GiaBan <= giaMax && ctp.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                group ctp by new { hx.HangXeID, hx.TenHangXe, ctp.DongXeID, dx.TenDongXe, ctp.DoiXe, ctp.DongCo } into groupc
                                select new CarTypeModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    CarImage = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).FirstOrDefault().CarImage,
                                    GiaMin = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Min(),
                                    GiaMax = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Max(),
                                    MauSacKhaDung = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.MauSac).Distinct().ToList()
                                }).ToList();
            }
            else if (TenHangXe != null && DongCo == null && MauSac != null)
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                listofDongXe = (from ctp in _context.CarType
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where dx.HangXeID == hxid && ctp.MauSac == MauSac && ctp.GiaBan >= giaMin && ctp.GiaBan <= giaMax && ctp.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                group ctp by new { hx.HangXeID, hx.TenHangXe, ctp.DongXeID, dx.TenDongXe, ctp.DoiXe, ctp.DongCo } into groupc
                                select new CarTypeModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    CarImage = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).FirstOrDefault().CarImage,
                                    GiaMin = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Min(),
                                    GiaMax = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Max(),
                                    MauSacKhaDung = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.MauSac).Distinct().ToList()
                                }).ToList();
            }
            else
            {
                var hxid = _context.HangXe.Where(hx => hx.TenHangXe.Equals(TenHangXe)).Select(hx => hx.HangXeID).FirstOrDefault();
                listofDongXe = (from ctp in _context.CarType
                                join dx in _context.DongXe on ctp.DongXeID equals dx.DongXeID
                                join hx in _context.HangXe on dx.HangXeID equals hx.HangXeID
                                where dx.HangXeID == hxid && ctp.MauSac == MauSac && ctp.DongCo == DongCo && ctp.GiaBan >= giaMin && ctp.GiaBan <= giaMax && ctp.IsDeleted == false && dx.IsDeleted == false && hx.IsDeleted == false
                                group ctp by new { hx.HangXeID, hx.TenHangXe, ctp.DongXeID, dx.TenDongXe, ctp.DoiXe, ctp.DongCo } into groupc
                                select new CarTypeModel
                                {
                                    HangXeID = groupc.Key.HangXeID,
                                    TenHangXe = groupc.Key.TenHangXe,
                                    DongXeID = groupc.Key.DongXeID,
                                    TenDongXe = groupc.Key.TenDongXe,
                                    DoiXe = groupc.Key.DoiXe,
                                    DongCo = groupc.Key.DongCo,
                                    CarImage = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).FirstOrDefault().CarImage,
                                    GiaMin = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Min(),
                                    GiaMax = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.GiaBan).Max(),
                                    MauSacKhaDung = _context.CarType.Where(c => c.DongXeID.Equals(groupc.Key.DongXeID) && c.DoiXe.Equals(groupc.Key.DoiXe) && c.DongCo.Equals(groupc.Key.DongCo)).Select(c => c.MauSac).Distinct().ToList()
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