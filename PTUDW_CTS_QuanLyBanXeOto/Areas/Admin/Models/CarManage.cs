using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models
{
    public class CarManage
    {
        //Tạo đối tượng để quản lý xe, dùng để hứng dữ liệu, không tương ứng bảng trong sql
        public int CarID { get; set; }
        public string TenHangXe { get; set; }
        public string TenDongXe { get; set; }
        public int DoiXe { get; set; }
        public string MauSac { get; set; }
        public string DongCo { get; set; }
        public int GiaBan { get; set; }
        public string CarImage { get; set; }
        public string VIN { get; set; }
        public int DongXeID { get; set; }
        public int PhieuXuatID { get; set; }
        public DateTime NgayNhap { get; set; }
        public int GiaNhap { get; set; }
        public int NguoiNhapID { get; set; }
        public string HoTenNguoiNhap { get; set; }
        public string TrangThai { get; set; }
    }
}
