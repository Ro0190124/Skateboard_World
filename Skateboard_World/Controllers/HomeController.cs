using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skateboard_World.Data;
using Skateboard_World.Models;
using System.Diagnostics;

namespace Skateboard_World.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
      

        public IActionResult Index()
        {
            // request cookies
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                // var sanPhamNoiBat = _context.db_CHI_TIET_GIO_HANG
                //.Include(x => x.SAN_PHAM).GroupBy(x => x.MaSP)
                //.Select(x => new SAN_PHAM { MaSP = x.Key, TenSP = x.First().SAN_PHAM.TenSP, GiaBan = x.First().SAN_PHAM.GiaBan, DS_HINH_ANH = x.First().SAN_PHAM.DS_HINH_ANH })
                //.Where(x=> )
                //.Take(6).ToList();
                var tatcachitiet = _context.db_CHI_TIET_GIO_HANG.ToList();
                Dictionary<int, int> sanpham_soluong = new Dictionary<int, int>();
                foreach (var item in tatcachitiet)
                {
                    if (sanpham_soluong.ContainsKey(item.MaSP))
                    {
                        sanpham_soluong[item.MaSP] += item.SoLuong;
                    }
                    else
                    {
                        sanpham_soluong.Add(item.MaSP, item.SoLuong);
                    }
                }
                var sanPhamNoiBat = sanpham_soluong.OrderByDescending(x => x.Value).Take(3).Select(x => _context.db_SAN_PHAM.Where(y => y.MaSP == x.Key).FirstOrDefault()).ToList();
                var hinhAnhSanPhamNoiBat = sanPhamNoiBat.Select(p => new HINH_ANH_SAN_PHAM
                {
                    SanPham = p,
                    HinhAnhList = _context.db_DS_HINH_ANH.Where(h => h.MaSP == p.MaSP).ToList()
                }).ToList();
                ViewData["SanPhamNoiBat"] = hinhAnhSanPhamNoiBat;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
        public IActionResult DangXuat()
        {
            HttpContext.Response.Cookies.Delete("UserID");
            HttpContext.Response.Cookies.Delete("Power");
            return RedirectToAction("Index", "DangNhap");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
