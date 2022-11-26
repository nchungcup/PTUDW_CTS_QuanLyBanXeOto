using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Models;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers
{
    //Tạo controller cho phần Profile của Admin
    [Area("Admin")]
    public class ProfileController : BaseController
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly DataContext _context;

        public ProfileController(ILogger<ProfileController> logger, DataContext dataContext) : base()
        {
            _logger = logger;
            _context = dataContext;
        }

        public IActionResult Profile()
        {
            var pf = (from u in _context.User
                      where u.Username == HttpContext.Session.GetString("username")
                      select u).ToList();
            return View(pf);
        }

        public async Task<IActionResult> Update(User user)
        {
            var updateu = new User
            {
                UserID = user.UserID,
                CMND = user.CMND,
                HoTen = user.HoTen,
                DiaChi = user.DiaChi,
                NamSinh = user.NamSinh,
                Email = user.Email,
                SoDienThoai = user.SoDienThoai,
                TypeID = _context.User.Where(u => u.UserID.Equals(user.UserID)).Select(u => u.TypeID).FirstOrDefault(),
                UserImage = _context.User.Where(u => u.UserID.Equals(user.UserID)).Select(u => u.UserImage).FirstOrDefault(),
                Username = _context.User.Where(u => u.UserID.Equals(user.UserID)).Select(u => u.Username).FirstOrDefault(),
                Password = _context.User.Where(u => u.UserID.Equals(user.UserID)).Select(u => u.Password).FirstOrDefault()
            };
            _context.User.Update(updateu);
            await _context.SaveChangesAsync();
            TempData["alertMessage"] = "Update Success!";
            return RedirectToAction("Profile", "Profile");
        }

        public IActionResult ChangePass()
        {
            var cp = (from u in _context.User
                      where u.Username == HttpContext.Session.GetString("username")
                      select u).ToList();
            return View(cp);
        }

        public async Task<IActionResult> UpdatePass(User _user, string NewPassword1, string NewPassword2)
        {
            if(_user.Password != _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.Password).FirstOrDefault())
            {
                TempData["alertMessage"] = "Wrong Old Password! Try Again";
                return RedirectToAction("ChangePass", "Profile");
            }    
            else if(NewPassword1 != NewPassword2)
            {
                TempData["alertMessage"] = "The New Password Re-entered The Second Time Is Wrong! Try Again";
                return RedirectToAction("ChangePass", "Profile");
            }
            else 
            { 
                var updatepass = new User
                {
                    UserID = _user.UserID,
                    CMND = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.CMND).FirstOrDefault(),
                    HoTen = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.HoTen).FirstOrDefault(),
                    DiaChi = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.DiaChi).FirstOrDefault(),
                    NamSinh = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.NamSinh).FirstOrDefault(),
                    Email = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.Email).FirstOrDefault(),
                    SoDienThoai = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.SoDienThoai).FirstOrDefault(),
                    TypeID = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.TypeID).FirstOrDefault(),
                    UserImage = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.UserImage).FirstOrDefault(),
                    Username = _context.User.Where(u => u.UserID.Equals(_user.UserID)).Select(u => u.Username).FirstOrDefault(),
                    Password = NewPassword1
                };
                _context.User.Update(updatepass);
                await _context.SaveChangesAsync();
                TempData["alertMessage"] = "Change Password Success!";
                return RedirectToAction("Profile", "Profile");
            }
        }
    }
}