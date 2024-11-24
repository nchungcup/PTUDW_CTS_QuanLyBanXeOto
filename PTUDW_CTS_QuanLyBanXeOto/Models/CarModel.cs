using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    public class CarModel
    {
        //Đối tượng lưu giữ thông tin về 1 chiếc xe trong danh mục
        public long HangXeID { get; set; }
        public long CarID { get; set; }
        public string TenHangXe { get; set; }
        public string TenDongXe { get; set; }
        public long DoiXe { get; set; }
        public string MauSac { get; set; }
        public string DongCo { get; set; }
        public long GiaBan { get; set; }
        public string CarImage { get; set; }
        public string VIN { get; set; }
        public long DongXeID { get; set; }
        public long SoLuong { get; set; }
        public long PhieuXuatID { get; set; }
        public DateTime NgayNhap { get; set; }
        public long GiaNhap { get; set; }
        public long NguoiNhapID { get; set; }
    }
}
