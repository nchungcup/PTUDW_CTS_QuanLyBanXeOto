using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models
{
    [Table("AdminMenu")]
    public class AdminMenu
    {
        //Tạo đối tượng tương ứng với bảng AdminMenu để hiện Menu trong Admin
        [Key]
        public int AdminMenuID { get; set; }
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public int ParentLevel { get; set; }
        public int ItemOrder { get; set; }
        public bool IsActive { get; set; }
        public string ItemTarget { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Icon { get; set; }
        public string IDName { get; set; }
    }
}
