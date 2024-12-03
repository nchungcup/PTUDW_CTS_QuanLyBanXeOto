using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    public class CarTypeModel
    {
        public long CarTypeID { get; set; }
        public string TenHangXe { get; set; }
        public long HangXeID { get; set; }
        public long DongXeID { get; set; }
        public string TenDongXe { get; set; }
        public long DoiXe { get; set; }
        public string MauSac { get; set; }
        public string DongCo { get; set; }
        public long GiaNhap { get; set; }
        public long GiaBan { get; set; }
        public long GiaMin { get; set; }
        public long GiaMax { get; set; }
        public string CarImage { get; set; }
        public long SoLuong { get; set; }
        public List<HangXeModel> ListHangXe { get; set; }
        public List<string> MauSacKhaDung { get; set; }
    }

}
