using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    public class Category
    { 
        public string TenHangXe { get; set; }
        public string TenDongXe { get; set; }
        public int DoiXe { get; set; }
        public string MauSac { get; set; }
        public string DongCo { get; set; }
        public int GiaBan { get; set; }
        public string CarImage { get; set; }
        public int VIN { get; set; }
        public int DongXeID { get; set; }
        public int PhieuXuatID { get; set; }
        public DateTime NgayNhap { get; set; }
        public int GiaNhap { get; set; }
        public int NguoiNhapID { get; set; }
    }
}
