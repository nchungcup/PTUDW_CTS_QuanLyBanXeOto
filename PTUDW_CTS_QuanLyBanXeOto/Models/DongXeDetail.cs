using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("DongXeDetail")]
    public class DongXeDetail
    {
        [Key]
        public int DongXeID { get; set; }
        public int HangXeID { get; set; }
        public string? TenDongXe { get; set; }
        public string? NoiSanXuat { get; set; }    
    }
}