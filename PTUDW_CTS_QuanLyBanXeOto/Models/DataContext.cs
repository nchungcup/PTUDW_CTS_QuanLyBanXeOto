using Microsoft.EntityFrameworkCore;
using PTUDW_CTS_QuanLyBanXeOto.Areas.Admin.Models;
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
        public DbSet<CarType> CarType { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<DongXeDetail> DongXe { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<AdminMenu> AdminMenus { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Cart> Cart { get; set; }
    }
}
