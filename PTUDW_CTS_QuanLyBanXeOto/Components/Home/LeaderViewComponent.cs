using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components
{
    //Tạo viewcomponent hiển thị các user là Admin trong Trang chủ
    [ViewComponent(Name = "Leader")]
    public class LeaderViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public LeaderViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Lấy dữ liệu từ sql
            var listofLeader = (from us in _context.User
                                where us.TypeID == 1 
                                select us).ToList();
            //Trả về view Leader với dữ liệu là listofLeader là những user là Admin trong sql
            return await Task.FromResult((IViewComponentResult)View("Leader", listofLeader));
        }
    }
}
