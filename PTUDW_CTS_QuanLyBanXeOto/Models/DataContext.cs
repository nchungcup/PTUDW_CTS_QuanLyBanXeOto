using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTUDW_CTS_QuanLyBanXeOto.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<HangXeDetail> HangXe { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<DongXeDetail> DongXe { get; set; }
        public DbSet<User> User { get; set; }
    }
}
