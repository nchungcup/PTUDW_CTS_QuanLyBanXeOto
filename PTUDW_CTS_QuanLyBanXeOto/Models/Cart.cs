using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public long CartID { get; set; }
        public long UserID { get; set; }
        public long CarTypeID { get; set; }
        public long SoLuong { get; set; }
    }
}