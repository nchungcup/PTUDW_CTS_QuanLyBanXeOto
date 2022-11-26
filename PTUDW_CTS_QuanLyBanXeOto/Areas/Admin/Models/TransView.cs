using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models
{
    public class TransView
    {
        public int TransID { get; set; }
        public int KhachHangID { get; set; }
        public string HoTenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime? NgayTaoDon { get; set; }
        public string ThanhToan { get; set; }
        public string TrangThai { get; set; }
        public int? NguoiXuLyID { get; set; }
        public string HoTenNguoiXuLy { get; set; }
        public DateTime? NgayDuyet { get; set; }
        public int? NguoiDuyetID { get; set; }
        public string HoTenNguoiDuyet { get; set; }
        public int ChietKhau { get; set; }
        public int Thue { get; set; }
        public int TongTien { get; set; }
    }
}
