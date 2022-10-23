using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public CategoryViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofCategory = (from hx in _context.HangXe
                                 join dx in _context.DongXe
                                 on hx.HangXeID equals dx.HangXeID
                                 orderby hx.TenHangXe
                                 select new
                                 {
                                     TenHangXe = hx.TenHangXe,
                                     TenDongXe = dx.TenDongXe
                                 }).AsEnumerable().Select(x => new Category()
                                 {
                                     TenHangXe = x.TenHangXe,
                                     TenDongXe = x.TenDongXe
                                 });

            return await Task.FromResult((IViewComponentResult)View("Default", listofCategory.ToList()));
        }
    }
}