using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    [Table("Car")]
    public class Car
    {
        [Key]
        public long CarID { get; set; }
        public long CarTypeID { get; set; }
        public string VIN { get; set; }
        public long? TransactionID { get; set; }
        public long? GiaBan { get; set; }
        public long? GiaNhap { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
