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
        public long TransID { get; set; }
        public long KhachHangID { get; set; }
        public DateTime NgayTaoDon { get; set; }
        public string ThanhToan { get; set; }
        public string TrangThai { get; set; }
        public long? NguoiXuLyID { get; set; }
        public DateTime? NgayDuyet { get; set; }
        public long? NguoiDuyetID { get; set; }
        public long ChietKhau { get; set; }
        public long TongTien { get; set; }
    }
}