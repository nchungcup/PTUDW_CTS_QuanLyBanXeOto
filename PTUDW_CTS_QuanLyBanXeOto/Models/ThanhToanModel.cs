using System.Collections.Generic;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    public class ThanhToanModel
    {
        public long TongTien { get; set; }
        public List<CartModel> CartView { get; set; }
    }
}
