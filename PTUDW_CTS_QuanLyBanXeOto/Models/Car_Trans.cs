using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("Cars-Trans")]
    public class Car_Trans
    {
        [Key]
        public long CarID { get; set; }
        public long? TransID { get; set; }
    }
}
