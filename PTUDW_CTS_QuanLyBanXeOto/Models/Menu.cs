using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public long MenuID { get; set; }
        public string MenuName { get; set; }
        public bool? IsActive { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public long? Levels { get; set; }
        public long? ParentID { get; set; }
        public string Link { get; set; }
        public long? MenuOrder { get; set; }
        public long? Position { get; set; }
    }
}
