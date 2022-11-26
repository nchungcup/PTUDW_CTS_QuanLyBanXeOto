using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers;

namespace PTUDW_CTS_QuanLyBanXeOto.Controllers
{
    public class ClientProfileController : ClientBaseController
    {
        private readonly ILogger<ClientProfileController> _logger;
        private readonly DataContext _context;

        public ClientProfileController(ILogger<ClientProfileController> logger, DataContext dataContext) : base()
        {
            _logger = logger;
            _context = dataContext;
        }

        public IActionResult Profile()
        {
            var pf = _context.User.Where(u => u.Username == HttpContext.Session.GetString("client")).FirstOrDefault();
            return View(pf);
        }

        public async Task<IActionResult> Update(User user)
        {
            var u = _context.User.Where(u => u.UserID.Equals(user.UserID)).FirstOrDefault();
            var updateu = new User
            {
                UserID = user.UserID,
                CMND = user.CMND,
                HoTen = user.HoTen,
                DiaChi = user.DiaChi,
                NamSinh = user.NamSinh,
                Email = user.Email,
                SoDienThoai = user.SoDienThoai,
                TypeID = u.TypeID,
                UserImage = u.UserImage,
                Username = u.Username,
                Password = u.Password
            };
            _context.User.Update(updateu);
            await _context.SaveChangesAsync();
            TempData["alertMessage"] = "Update Success!";
            return RedirectToAction("Profile", "ClientProfile");
        }

        public IActionResult ChangePass()
        {
            var cp = _context.User.Where(u => u.Username == HttpContext.Session.GetString("client")).FirstOrDefault();
            return View(cp);
        }

        public async Task<IActionResult> UpdatePass(User _user, string NewPassword1, string NewPassword2)
        {
            if (_user.Password != _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.Password).FirstOrDefault())
            {
                TempData["alertMessage"] = "Wrong Old Password! Try Again";
                return RedirectToAction("ChangePass", "ClientProfile");
            }
            else if (NewPassword1 != NewPassword2)
            {
                TempData["alertMessage"] = "The New Password Re-entered The Second Time Is Wrong! Try Again";
                return RedirectToAction("ChangePass", "ClientProfile");
            }
            else
            {
                var up = _context.User.Where(u => u.UserID.Equals(_user.UserID)).FirstOrDefault();
                var updatepass = new User
                {
                    UserID = _user.UserID,
                    CMND = up.CMND,
                    HoTen = up.HoTen,
                    DiaChi = up.DiaChi,
                    NamSinh = up.NamSinh,
                    Email = up.Email,
                    SoDienThoai =up.SoDienThoai,
                    TypeID = up.TypeID,
                    UserImage = up.UserImage,
                    Username = up.Username,
                    Password = NewPassword1
                };
                _context.User.Update(updatepass);
                await _context.SaveChangesAsync();
                TempData["alertMessage"] = "Change Password Success!";
                return RedirectToAction("Profile", "ClientProfile");
            }
        }
    }
}