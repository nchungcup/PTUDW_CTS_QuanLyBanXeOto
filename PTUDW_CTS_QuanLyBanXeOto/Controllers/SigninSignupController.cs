using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Controllers;
using Microsoft.Extensions.Configuration;

namespace PTUDW_CTS_QuanLyBanXeOto.Controllers
{
    public class SigninSignupController : Controller
    {
        private readonly ILogger<SigninSignupController> _logger;
        private readonly DataContext _context;
        public SigninSignupController(ILogger<SigninSignupController> logger, DataContext datacontext)
        {
            _logger = logger;
            _context = datacontext;
        }
        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signin(string username, string password, string remember = "off")
        {
            var data = _context.User.Where(u => u.Username.Equals(username) && u.Password.Equals(password)).ToList();
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(1));
            CookieOptions cookieOptions1 = new CookieOptions();
            cookieOptions1.Expires = new DateTimeOffset(DateTime.Now.AddDays(-1));
            if (data.Count() > 0)
            {
                var usession = (from u in _context.User
                            where u.Username == username && u.Password == password select u).FirstOrDefault();
                if(usession.TypeID == 1)
                {
                    if (remember == "on")
                    {
                        HttpContext.Response.Cookies.Append("username", username, cookieOptions);
                        HttpContext.Response.Cookies.Append("password", password, cookieOptions);
                        HttpContext.Response.Cookies.Append("remember", remember, cookieOptions);
                    }
                    else
                    {
                        if(HttpContext.Request.Cookies["username"] != null && HttpContext.Request.Cookies["password"] != null)
                        {     
                        HttpContext.Response.Cookies.Append("username", username, cookieOptions1);
                        HttpContext.Response.Cookies.Append("password", password, cookieOptions1);
                        HttpContext.Response.Cookies.Append("remember", remember, cookieOptions1);
                        }
                    }
                    HttpContext.Session.SetString("username", username);
                    HttpContext.Session.Remove("client");
                    return RedirectToAction("Index", "Home", new { area = "Admin" } );
                }    
                else
                {
                    if (!usession.IsActive)
                    {
                        TempData["alertMessage"] = "Tài khoản của bạn đã bị khóa! Vui lòng liên hệ quản trị viên!";
                        return RedirectToAction("Signin", "SigninSignup");
                    } 
                        
                    if (remember == "on")
                    {
                        HttpContext.Response.Cookies.Append("username", username, cookieOptions);
                        HttpContext.Response.Cookies.Append("password", password, cookieOptions);
                        HttpContext.Response.Cookies.Append("remember", remember, cookieOptions);
                    }
                    else
                    {
                        if (HttpContext.Request.Cookies["username"] != null && HttpContext.Request.Cookies["password"] != null)
                        {
                            HttpContext.Response.Cookies.Append("username", username, cookieOptions1);
                            HttpContext.Response.Cookies.Append("password", password, cookieOptions1);
                            HttpContext.Response.Cookies.Append("remember", remember, cookieOptions1);
                        }
                    }
                    HttpContext.Session.SetString("client", username);
                    HttpContext.Session.Remove("username");
                    return RedirectToAction("Index", "Home");
                }    

             }    
            else
            {
                var u = _context.User.Where(u => u.Username.Equals(username) && u.Password != password).Select(u => u).Count();
                if ( u == 1)
                {
                    TempData["alertMessage"] = "Mật khẩu sai!";
                    return RedirectToAction("Signin", "SigninSignup");
                }    
                TempData["alertMessage"] = "Tài khoản không tồn tại!";
                return RedirectToAction("Signin","SigninSignup");
            }                
        }
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup(User _user)
        {
            var userduplicate = _context.User.Where(u => u.Username.Equals(_user.Username)).ToList();
            var emailduplicate = _context.User.Where(u => u.Username.Equals(_user.Email)).ToList();
            var phoneduplicate = _context.User.Where(u => u.Username.Equals(_user.SoDienThoai)).ToList();
            var CMNDduplicate = _context.User.Where(u => u.Username.Equals(_user.CMND)).ToList();
            if(userduplicate.Count() > 0)
            {
                TempData["alertMessage"] = "Tên đăng nhập đã tồn tại!";
                return RedirectToAction("Signup", "SigninSignup");
            }  
            else if(emailduplicate.Count() > 0)
            {
                TempData["alertMessage"] = "Email đã tồn tại!";
                return RedirectToAction("Signup", "SigninSignup");
            }    
            else if(phoneduplicate.Count() > 0)
            {
                TempData["alertMessage"] = "Số điện thoại đã tồn tại!";
                return RedirectToAction("Signup", "SigninSignup");
            }    
            else if(CMNDduplicate.Count() > 0)
            {
                TempData["alertMessage"] = "Số CMND/CCCD đã tồn tại!";
                return RedirectToAction("Signup", "SigninSignup");
            }    
            else
            {
                var us = new User
                {
                    CMND = _user.CMND,
                    HoTen = _user.HoTen,
                    DiaChi = _user.DiaChi,
                    NamSinh = _user.NamSinh,
                    Email = _user.Email,
                    SoDienThoai = _user.SoDienThoai,
                    Username = _user.Username,
                    Password = _user.Password,
                    IsActive = true
                };

                _context.User.Add(us);
                await _context.SaveChangesAsync();
                return RedirectToAction("Signin", "SigninSignup");
            }                
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("client");
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
