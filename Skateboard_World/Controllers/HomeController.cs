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


        public async Task<IActionResult> Index()
        {
            // Request cookies
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                var today = DateTime.Now.Month;
                // Retrieve completed orders
                var hoaDonQuery = _context.db_HOA_DON.AsQueryable();
                var hoaDon = await hoaDonQuery.Where(hd => hd.TrangThai == 3 && hd.NgayTao.Month == today).ToListAsync();
                var totalOrders = hoaDon.Count;

                // Get the list of MaGioHang from completed orders
                var completedOrderIds = hoaDon.Select(hd => hd.MaGioHang).Distinct().ToList();

                // Get the list of chi tiet gio hang related to completed orders
                var chiTietGioHangs = await _context.db_CHI_TIET_GIO_HANG
                    .Include(ctgh => ctgh.SAN_PHAM)
                    .Where(ctgh => completedOrderIds.Contains(ctgh.MaGioHang))
                    .ToListAsync();

                // Calculate total revenue
                double totalRevenue = chiTietGioHangs.Sum(ctgh => ctgh.SAN_PHAM.GiaBan * ctgh.SoLuong);

                // Create a DOT object to store the statistics
                var dot = new DOT
                {
                    TotalOrders = totalOrders,
                    TotalRevenue = totalRevenue,
                };
                ViewData["DOT"] = dot;

                // Retrieve all chi tiet gio hang
                var tatcachitiet = await _context.db_CHI_TIET_GIO_HANG.ToListAsync();
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

                // Retrieve top 3 san pham noi bat
                var sanPhamNoiBat = sanpham_soluong.OrderByDescending(x => x.Value).Take(3).Select(x => new
                {
                    Product = _context.db_SAN_PHAM.FirstOrDefault(y => y.MaSP == x.Key),
                    TotalQuantitySold = x.Value
                })
                .ToList();

                var hinhAnhSanPhamNoiBat = sanPhamNoiBat.Select(p => new HINH_ANH_SAN_PHAM
                {
                    SanPham = p.Product,
                    HinhAnhList = _context.db_DS_HINH_ANH.Where(h => h.MaSP == p.Product.MaSP).ToList(),
                    TotalQuantitySold = p.TotalQuantitySold
                }).ToList();
                ViewData["SanPhamNoiBat"] = hinhAnhSanPhamNoiBat;

                return View(sanpham_soluong);
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
