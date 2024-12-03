using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public long UserID { get; set; }
        public string CMND { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public long NamSinh { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public long TypeID { get; set; }
        public string UserImage { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
