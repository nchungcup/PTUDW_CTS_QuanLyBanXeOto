using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models
{
    public class CarManage
    {
        //Tạo đối tượng để quản lý xe, dùng để hứng dữ liệu, không tương ứng bảng trong sql
        public long CarID { get; set; }
        public string TenHangXe { get; set; }
        public long HangXeID { get; set; }
        public string TenDongXe { get; set; }
        public long DongXeID { get; set; }
        public long DoiXe { get; set; }
        public string MauSac { get; set; }
        public string DongCo { get; set; }
        public long GiaBan { get; set; }
        public string CarImage { get; set; }
        public string VIN { get; set; }
        public long? PhieuXuatID { get; set; }
        public DateTime NgayNhap { get; set; }
        public long GiaNhap { get; set; }
        public long NguoiNhapID { get; set; }
        public string Username { get; set; }
        public string TrangThai { get; set; }
        public List<HangXe> ListHangXe { get; set; }
    }
    public class HangXe
    {
        public long HangXeID { get; set; }
        public string TenHangXe { get; set; }
    }    
        
}
