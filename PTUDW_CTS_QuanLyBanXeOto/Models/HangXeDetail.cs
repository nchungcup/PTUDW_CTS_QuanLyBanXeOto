using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("HangXeDetail")]
    public class HangXeDetail
    {
        [Key]
        public int HangXeID { get; set; }
        public string TenHangXe { get; set; }
        public string XuatXu { get; set; }
        public string LogoImage { get; set; }
    }
}