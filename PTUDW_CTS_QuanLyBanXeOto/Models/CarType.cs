using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("CarType")]
    public class CarType
    {
        [Key]
        public long CarTypeID { get; set; }
        public long DongXeID { get; set; }
        public long DoiXe { get; set; }
        public string MauSac { get; set; }
        public string DongCo { get; set; }
        public string CarImage { get; set; }
        public long GiaNhap { get; set; }
        public long GiaBan { get; set; }
        public bool? IsDeleted { get; set; }

    }
}