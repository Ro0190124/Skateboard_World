using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skateboard_World.Data;
using Skateboard_World.Models;

namespace Skateboard_World.Controllers
{
    public class USER_SAN_PHAM : Controller
    {
        private readonly ApplicationDbContext _context;

        public USER_SAN_PHAM(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/UserIndex")]
        public IActionResult Index()
        {
            var products = _context.db_SAN_PHAM.Where(x => x.TrangThai == true).ToList();

            var productWithImages = products.Select(p => new HINH_ANH_SAN_PHAM
            {
                SanPham = p,
                HinhAnhList = _context.db_DS_HINH_ANH.Where(h => h.MaSP == p.MaSP).ToList()
            }).ToList();
            var sanPhamNoiBat = _context.db_CHI_TIET_GIO_HANG
                .Include(x => x.SAN_PHAM).GroupBy(x => x.MaSP)
                .Select(x => new SAN_PHAM { MaSP = x.Key, TenSP = x.First().SAN_PHAM.TenSP, GiaBan = x.First().SAN_PHAM.GiaBan, DS_HINH_ANH = x.First().SAN_PHAM.DS_HINH_ANH })
                .Take(6).ToList();
            var hinhAnhSanPhamNoiBat = sanPhamNoiBat.Select(p => new HINH_ANH_SAN_PHAM
            {
                SanPham = p,
                HinhAnhList = _context.db_DS_HINH_ANH.Where(h => h.MaSP == p.MaSP).ToList()
            }).ToList();
            ViewData["SanPhamNoiBat"] = hinhAnhSanPhamNoiBat;
            return View(productWithImages);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sAN_PHAM = await _context.db_SAN_PHAM
                .Include(s => s.DS_HINH_ANH)
                .FirstOrDefaultAsync(m => m.MaSP == id);
            if (sAN_PHAM == null)
            {
                return NotFound();
            }
            var danhSachSP = _context.db_SAN_PHAM
          .Where(x => x.TrangThai == true)
          .Include(x => x.DS_HINH_ANH) // Include related images if needed
          .ToList();

            ViewData["DanhSachSP"] = danhSachSP;
            Console.WriteLine("Danh sach hinh anh: " + sAN_PHAM.DS_HINH_ANH.Count());


            return View(sAN_PHAM);
        }
        public IActionResult Product()
        {
            var products = _context.db_SAN_PHAM.Where(x => x.TrangThai == true).ToList();

            var productWithImages = products.Select(p => new HINH_ANH_SAN_PHAM
            {
                SanPham = p,
                HinhAnhList = _context.db_DS_HINH_ANH.Where(h => h.MaSP == p.MaSP).ToList()
            }).ToList();

            return View(productWithImages);
        }
    }
}
