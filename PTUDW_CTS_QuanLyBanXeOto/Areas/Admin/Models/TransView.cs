using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models
{
    public class TransView
    {
        public long TransID { get; set; }
        public long KhachHangID { get; set; }
        public string HoTenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime? NgayTaoDon { get; set; }
        public string ThanhToan { get; set; }
        public string TrangThai { get; set; }
        public long? NguoiXuLyID { get; set; }
        public string HoTenNguoiXuLy { get; set; }
        public DateTime? NgayDuyet { get; set; }
        public long? NguoiDuyetID { get; set; }
        public string HoTenNguoiDuyet { get; set; }
        public long ChietKhau { get; set; }
        public long Thue { get; set; }
        public long TongTien { get; set; }
    }
}
