using Microsoft.AspNetCore.Mvc;
using PTUDW_CTS_QuanLyBanXeOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Components
{
    [ViewComponent(Name = "Slide")]
    public class SlideComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}
