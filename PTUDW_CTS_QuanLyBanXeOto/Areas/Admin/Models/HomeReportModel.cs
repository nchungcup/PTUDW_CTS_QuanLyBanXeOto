using System.Collections.Generic;

namespace PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models
{
    public class HomeReportModel
    {
        public List<string> Time { get; set; }
        public List<long> TransactionCount { get; set; }
        public List<long> Profit { get; set; }

    }
}
