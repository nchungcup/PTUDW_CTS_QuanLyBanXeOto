using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PTUDW_CTS_QuanLyBanXeOto.Components.Gallery
{
    //Component Gallery để hiển thị bộ sưu tập
    [ViewComponent(Name = "Gallery")]
    public class GalleryViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public GalleryViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Lấy dữ liệu từ sql
            var listofGallery = _context.HangXe.Where(hx => hx.IsDeleted == false).ToList();
            //Trả về view default với dữ liệu là listofGallery
            return await Task.FromResult((IViewComponentResult)View("Default", listofGallery));
        }
    }
}
