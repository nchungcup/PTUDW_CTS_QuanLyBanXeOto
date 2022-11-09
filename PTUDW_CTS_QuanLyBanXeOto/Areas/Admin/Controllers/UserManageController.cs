using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers
{
    //Tạo controller cho phần quản lý người dùng trong Admin
    [Area("Admin")]
    public class UserManageController : Controller
    {
        private readonly ILogger<UserManageController> _logger;
        private readonly DataContext _context;

        public UserManageController(ILogger<UserManageController> logger, DataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }
        //Method trả về view quản lý người dùng
        public IActionResult UserManage()
        {
            //Lấy dữ liệu từ sql
            var ctmlist = (from ctm in _context.User
                           where ctm.TypeID == 2
                           orderby ctm.UserID
                           select ctm).ToList();
            return View(ctmlist);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
