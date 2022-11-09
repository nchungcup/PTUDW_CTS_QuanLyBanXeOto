using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Components
{
    //Tạo viewcomponent cho menu của Admin
    [ViewComponent(Name = "AdminMenu")]
    public class AdminMenuComponent : ViewComponent
    {
        private readonly DataContext _context;
        public AdminMenuComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Lấy dữ liệu từ bảng AdminMenu trong sql
            var listofuser = (from mn in _context.AdminMenus
                              where (mn.IsActive == true)
                              select mn).ToList();
            //Trả về view menu có tên Default với dữ liệu là listofUser
            return await Task.FromResult((IViewComponentResult)View("Default", listofuser));
        }
    }
}