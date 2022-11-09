using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int TransID { get; set; }
        public int KhachHangID { get; set; }
        public int MaHoaDon { get; set; }
        public DateTime NgayTaoDon { get; set; }
        public string ThanhToan { get; set; }
        public string TrangThai { get; set; }
        public int NguoiXuLyID { get; set; }
        public DateTime NgayDuyet { get; set; }
        public int NguoiDuyetID { get; set; }
        public int ChietKhau { get; set; }
        public int Thue { get; set; }
    }
}