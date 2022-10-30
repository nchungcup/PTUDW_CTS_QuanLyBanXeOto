using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components
{
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
            var listofLeader = (from us in _context.User
                                where us.TypeID == 1 
                                select us).ToList();
            return await Task.FromResult((IViewComponentResult)View("Leader", listofLeader));
        }
    }
}
