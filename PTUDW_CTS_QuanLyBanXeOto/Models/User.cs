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
        public int UserID { get; set; }
        public int CMND { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public int NamSinh { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public int TypeID { get; set; }
        public string UserImage { get; set; }
    }
}
