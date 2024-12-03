using System.Collections.Generic;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    public class HangXeModel
    {
        public long HangXeID { get; set; }
        public string TenHangXe { get; set; }
        public string XuatXu { get; set; }
        public string LogoImage { get; set; }
        public List<DongXeModel> ListDongXe { get; set; }
    }
}
