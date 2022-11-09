using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Components
{
    // Tạo viewcomponent cho phần thông tin Admin
    [ViewComponent(Name = "AdminInfo")]
    public class AdminInfoViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public AdminInfoViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessionofAdmin = (from adm in _context.User
                              where adm.TypeID == 1 && adm.Username == HttpContext.Session.GetString("username")
                              select adm).ToList();
            // Trả về view default trong AdminInfo
            return await Task.FromResult((IViewComponentResult)View("Default", sessionofAdmin));
        }
    }
}
