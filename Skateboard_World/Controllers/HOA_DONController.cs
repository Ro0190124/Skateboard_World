using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skateboard_World.Data;
using Skateboard_World.Models;
using System.Net;

namespace Skateboard_World.Controllers
{
    public class HOA_DONController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HOA_DONController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Admin_Index(string value)
        {
            
            if (value == null)
            {
                value = "0";
            }
            
            TempData["currentValue"] = value;

            string? userID = HttpContext.Request.Cookies["UserID"];

            if (userID == null)
            {
                TempData["DangNhap_User"] = "Vui lòng đăng nhập";
                return Redirect(Request.Headers["Referer"].ToString());
              
            }
            else
            {
                NGUOI_DUNG? nguoiDung = _context.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(userID)).First();
                TempData["PhanQuyen"] = nguoiDung.PhanQuyen;
                IEnumerable<HOA_DON> obj;

                if (nguoiDung.PhanQuyen == true)
                {
                    obj = _context.db_HOA_DON.Where(x => x.TrangThai == int.Parse(value)).Include(x => x.GIO_HANG).ThenInclude(x => x.NGUOI_DUNG).ToList();
                }
                else
                {
                    obj = _context.db_HOA_DON.Where(x => x.GIO_HANG.MaNguoiDung == int.Parse(userID) && x.TrangThai == int.Parse(value)).Include(x => x.GIO_HANG).ThenInclude(x => x.NGUOI_DUNG).ToList();
                }
                List<double> total = new List<double>();

                foreach (var item in obj)
                {
                    double totalPrice = 0;
                    var sp = _context.db_CHI_TIET_GIO_HANG
                                        .Include(x => x.GIO_HANG)
                                        .Where(x => x.GIO_HANG.MaGioHang == item.MaGioHang)
                                        .Select(x => new { Price = x.SAN_PHAM.GiaBan, Quantity = x.SoLuong })
                                        .ToList();


                    foreach (var i in sp)
                    {
                        totalPrice += i.Price * i.Quantity;
                    }

                    total.Add(totalPrice);
                }

                ViewBag.TotalPrices = total;
                if(nguoiDung.PhanQuyen == true)
                {
                    return View(obj);
                }
                else
                {
                    return View(obj);
                }

            }
           
        }
        public async Task<IActionResult> ChiTietDonHang(int? id, string value)
        {
            Console.WriteLine("value : " + value);
            if (value == null)
            {
                value = "0";
            }
            TempData["currentValue"] = value;
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                NGUOI_DUNG? KhachHang = _context.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(userID) && x.PhanQuyen == true && x.TrangThai == true).FirstOrDefault();
                NGUOI_DUNG? user = _context.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(userID) && x.PhanQuyen == false && x.TrangThai == true).FirstOrDefault();
                if (user != null)
                {
                    TempData["PhanQuyen"] = user.PhanQuyen;
                    HOA_DON hoaDon = _context.db_HOA_DON.Where(x => x.MaHD == id).FirstOrDefault();
                    TempData["MaHoaDon"] = hoaDon.MaHD;
                    TempData["TrangThai"] = hoaDon.TrangThai;
                    TempData["GhiChu"] = hoaDon.GhiChu;

                    /* var gioHang = _context.db_CHI_TIET_GIO_HANG
                         .Include(x => x.GIO_HANG)
                         .Where(x => x.GIO_HANG.MaGioHang == hoaDon.MaGioHang)
                         .FirstOrDefault();*/
                    var gioHangChiTiet = _context.db_GIO_HANG.Where(x => x.MaGioHang == hoaDon.MaGioHang).FirstOrDefault();

                    var nguoiDung = _context.db_NGUOI_DUNG.Where(x => x.MaND == gioHangChiTiet.MaNguoiDung).FirstOrDefault();
                    TempData["NguoiNhan"] = nguoiDung;

                    if (gioHangChiTiet != null)
                    {
                        var applicationDbContext = _context.db_CHI_TIET_GIO_HANG
                            .Include(c => c.GIO_HANG)
                            .Include(c => c.SAN_PHAM)
                            .ThenInclude(x => x.DS_HINH_ANH)
                            .Include(x => x.GIO_HANG)
                            .Where(x => x.GIO_HANG.MaGioHang == gioHangChiTiet.MaGioHang)
                            ;
                        return View(await applicationDbContext.ToListAsync());
                    }
                }
                if (KhachHang == null)
                {
                    TempData["DangNhap_User"] = "Vui lòng đăng nhập";
                    return View();
                }
                else
                {

                    HOA_DON hoaDon = _context.db_HOA_DON.Where(x => x.MaHD == id).FirstOrDefault();
                    TempData["MaHoaDon"] = hoaDon.MaHD;
                    TempData["TrangThai"] = hoaDon.TrangThai;
                    TempData["PhanQuyen"] = KhachHang.PhanQuyen;
                    TempData["GhiChu"] = hoaDon.GhiChu;

                    /* var gioHang = _context.db_CHI_TIET_GIO_HANG
                         .Include(x => x.GIO_HANG)
                         .Where(x => x.GIO_HANG.MaGioHang == hoaDon.MaGioHang)
                         .FirstOrDefault();*/
                    var gioHangChiTiet = _context.db_GIO_HANG.Where(x => x.MaGioHang == hoaDon.MaGioHang).FirstOrDefault();

                    var nguoiDung = _context.db_NGUOI_DUNG.Where(x => x.MaND == gioHangChiTiet.MaNguoiDung).FirstOrDefault();
                    TempData["NguoiNhan"] = nguoiDung;

                    if (gioHangChiTiet != null)
                    {
                        var applicationDbContext = _context.db_CHI_TIET_GIO_HANG
                            .Include(c => c.GIO_HANG)
                            .Include(c => c.SAN_PHAM)
                            .ThenInclude(x => x.DS_HINH_ANH)
                            .Include(x => x.GIO_HANG)
                            .Where(x => x.GIO_HANG.MaGioHang == gioHangChiTiet.MaGioHang)
                            ;
                        return View(await applicationDbContext.ToListAsync());
                    }
                }
            }
            else
            {
                TempData["DangNhap_User"] = "Vui lòng đăng nhập";

            }
            return View();
        }
        public ActionResult XacNhan(int id)
        {
            HOA_DON hoaDon = _context.db_HOA_DON.Where(x => x.MaHD == id).First();
            hoaDon.TrangThai = 1;
            _context.SaveChanges();
            TempData["tbDatHang"] = "Đã xác nhận đơn hàng";
            ViewBag.CurrentValue = "0";
            TempData["currentValue"] = 0;
            return RedirectToAction("Admin_Index", "HOA_DON", new { value = TempData["currentValue"] });

        }
        public ActionResult Huy(int? id)
        {
            HOA_DON hoaDon = _context.db_HOA_DON.Where(x => x.MaHD == id).First();
            hoaDon.TrangThai = 4;
            _context.SaveChanges();
            TempData["tbDatHang"] = "Đã hủy đơn hàng";
            ViewBag.CurrentValue = "";
            TempData["currentValue"] = 0;
            return RedirectToAction("Admin_Index", "HOA_DON", new { value = TempData["currentValue"] });

        }
        public ActionResult GiaoHang(int id)
        {
            HOA_DON hoaDon = _context.db_HOA_DON.Where(x => x.MaHD == id).First();
            hoaDon.TrangThai = 2;
            _context.SaveChanges();
            TempData["tbDatHang"] = "Đã giao hàng";
            ViewBag.CurrentValue = "1";
            TempData["currentValue"] = 1;
            return RedirectToAction("Admin_Index", "HOA_DON", new { value = TempData["currentValue"] });

        }
        public ActionResult DaGiao(int id)
        {
            HOA_DON hoaDon = _context.db_HOA_DON.Where(x => x.MaHD == id).First();
            hoaDon.TrangThai = 3;
            _context.SaveChanges();
            TempData["tbDatHang"] = "Đã nhận hàng";
            ViewBag.CurrentValue = "2";
            TempData["currentValue"] = 2;
            return RedirectToAction("Admin_Index", "HOA_DON", new { value = TempData["currentValue"] });

        }
        /* public IActionResult User_Index(string value)
         {
             if (value == null)
             {
                 value = "0";
             }

             TempData["currentValue"] = value;

             string? userID = HttpContext.Request.Cookies["UserID"];

             if (userID == null)
             {
                 return NotFound();
             }
             else
             {
                 NGUOI_DUNG? nguoiDung = _context.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(userID)).First();

                 IEnumerable<HOA_DON> obj;

                 if (nguoiDung.PhanQuyen == true)
                 {
                     obj = _context.db_HOA_DON.Where(x => x.TrangThai == int.Parse(value)).Include(x => x.GIO_HANG).ThenInclude(x => x.NGUOI_DUNG).ToList();
                 }
                 else
                 {
                     obj = _context.db_HOA_DON.Where(x => x.GIO_HANG.MaNguoiDung == int.Parse(userID) && x.TrangThai == int.Parse(value)).Include(x => x.GIO_HANG).ThenInclude(x => x.NGUOI_DUNG).ToList();
                 }
                 List<double> total = new List<double>();

                 foreach (var item in obj)
                 {
                     double totalPrice = 0;
                     var sp = _context.db_CHI_TIET_GIO_HANG
                                         .Include(x => x.GIO_HANG)
                                         .Where(x => x.GIO_HANG.MaGioHang == item.MaGioHang)
                                         .Select(x => new { Price = x.SAN_PHAM.GiaBan, Quantity = x.SoLuong })
                                         .ToList();


                     foreach (var i in sp)
                     {
                         totalPrice += i.Price * i.Quantity;
                     }

                     total.Add(totalPrice);
                 }

                 ViewBag.TotalPrices = total;
                 if (nguoiDung.PhanQuyen == true)
                 {
                     return View(obj);
                 }
                 else
                 {
                     return View("User_Index", obj);
                 }

             }
         }*/

        /* public async Task<IActionResult> SalesStatistics()
         {
             // Tính tổng số đơn hàng đã hoàn thành (TrangThai == 3)
             var hoaDon = _context.db_HOA_DON.ToList().Where(hd => hd.TrangThai == 3);
             var totalOrders = hoaDon.Count();
             // Tính tổng doanh thu từ các đơn hàng đã hoàn thành
             // tìm giỏ hàng của đơn hàng đã hoàn thành
             var gioHangs =  _context.db_GIO_HANG
                 .Where(gh => _context.db_HOA_DON.Any(hd => hd.MaGioHang == gh.MaGioHang && hd.TrangThai == 3))
                 .ToList();
             // tìm chi tiết giỏ hàng đã hoản thành
             var chiTietGioHangs = _context.db_CHI_TIET_GIO_HANG
                 .Where(ctgh => gioHangs.Any(gh => gh.MaGioHang == ctgh.MaGioHang))
                 .ToList();
             // tính tổng doanh thu
             double totalRevenue = 0;
             foreach (var ctgh in chiTietGioHangs)
             {
                 totalRevenue += ((ctgh.SAN_PHAM.GiaBan) * ctgh.SoLuong);
             }


             // Tạo đối tượng DOT để lưu trữ các thống kê
             var dot = new DOT
             {
                 TotalOrders = totalOrders,
                 TotalRevenue = totalRevenue,
             };

             return View(dot); // Truyền đối tượng DOT đến view
         }*/
        public async Task<IActionResult> SalesStatistics(DateTime? fromDate, DateTime? toDate)
        {
            // Lấy danh sách hóa đơn đã hoàn thành (TrangThai == 3)
            var hoaDonQuery = _context.db_HOA_DON.AsQueryable();

            if (fromDate.HasValue)
            {
                hoaDonQuery = hoaDonQuery.Where(hd => hd.NgayTao >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                hoaDonQuery = hoaDonQuery.Where(hd => hd.NgayTao <= toDate.Value);
            }

            var hoaDon = await hoaDonQuery.Where(hd => hd.TrangThai == 3).ToListAsync();
            var totalOrders = hoaDon.Count();

            // Lấy danh sách các MaGioHang từ các hóa đơn đã hoàn thành
            var completedOrderIds = hoaDon.Select(hd => hd.MaGioHang).Distinct().ToList();

            // Lấy danh sách các chi tiết giỏ hàng liên quan đến các giỏ hàng trong các hóa đơn đã hoàn thành
            var chiTietGioHangs = await _context.db_CHI_TIET_GIO_HANG
                .Include(ctgh => ctgh.SAN_PHAM)
                .Where(ctgh => completedOrderIds.Contains(ctgh.MaGioHang))
                .ToListAsync();

            // Tính tổng doanh thu
            double totalRevenue = chiTietGioHangs.Sum(ctgh => ctgh.SAN_PHAM.GiaBan * ctgh.SoLuong);

            // Tạo đối tượng DOT để lưu trữ các thống kê
            var dot = new DOT
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
            };

            return View(dot); // Truyền đối tượng DOT đến view
        }
    }
}
