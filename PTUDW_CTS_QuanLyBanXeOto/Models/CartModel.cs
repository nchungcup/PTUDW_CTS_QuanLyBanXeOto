using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    public class CartModel
    {
        [Key]
        public long UserID { get; set; }
        public long DongXeID { get; set; }
        public long HangXeID { get; set; }
        public string CarImage { get; set; }
        public string TenDongXe { get; set; }
        public string TenHangXe { get; set; }
        public string MauSac { get; set; }
        public long DoiXe { get; set; }
        public string DongCo { get; set; }
        public long GiaBan { get; set; }
        public long SoLuong { get; set; }
    }
}