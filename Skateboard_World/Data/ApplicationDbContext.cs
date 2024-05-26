using Microsoft.EntityFrameworkCore;
using Skateboard_World.Models;

namespace Skateboard_World.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<NGUOI_DUNG> db_NGUOI_DUNG { get; set; }
        public DbSet<SAN_PHAM> db_SAN_PHAM { get; set; }
        public DbSet<DS_HINH_ANH> db_DS_HINH_ANH { get; set; }
        public DbSet<GIO_HANG> db_GIO_HANG { get; set; }
        public DbSet<CHI_TIET_GIO_HANG> db_CHI_TIET_GIO_HANG { get; set; }
        public DbSet<HOA_DON> db_HOA_DON { get; set; }
    }
}
