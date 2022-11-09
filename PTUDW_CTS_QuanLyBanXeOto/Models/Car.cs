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
        public int CarID { get; set; }
        public string VIN { get; set; }
        public int DongXeID { get; set; }
        public int DoiXe { get; set; }
        public string MauSac { get; set; }
        public string DongCo { get; set; }
        public int GiaBan { get; set; }
        public string CarImage { get; set; }
        public int PhieuXuatID { get; set; }
        public DateTime NgayNhap { get; set; }
        public int GiaNhap { get; set; }
        public int NguoiNhapID { get; set; }

    }
}