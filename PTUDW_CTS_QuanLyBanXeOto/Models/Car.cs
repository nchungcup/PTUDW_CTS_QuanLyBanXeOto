using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("Car")]
    public class Car
    {
        [Key]
        public long CarID { get; set; }
        public string VIN { get; set; }
        public long DongXeID { get; set; }
        public long DoiXe { get; set; }
        public string MauSac { get; set; }
        public string DongCo { get; set; }
        public string CarImage { get; set; }
        public long? PhieuXuatID { get; set; }
        public DateTime NgayNhap { get; set; }
        public long NguoiNhapID { get; set; }
        public long GiaNhap { get; set; }
        public long GiaBan { get; set; }
    }
}