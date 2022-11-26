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
        public int CartID { get; set; }
        public int UserID { get; set; }
        public int CarID { get; set; }
    }
}