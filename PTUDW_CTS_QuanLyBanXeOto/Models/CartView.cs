using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    public class CartView
    {
        [Key]
        public int UserID { get; set; }
        public int DongXeID { get; set; }
        public string CarImage { get; set; }
        public string TenDongXe { get; set; }
        public string MauSac { get; set; }
        public int DoiXe { get; set; }
        public string DongCo { get; set; }
        public int GiaBan { get; set; }
        public int SoLuong { get; set; }

    }
}