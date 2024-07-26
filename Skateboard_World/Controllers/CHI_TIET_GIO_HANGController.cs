using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skateboard_World.Data;
using Skateboard_World.Models;

namespace Skateboard_World.Controllers
{
    public class CHI_TIET_GIO_HANGController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CHI_TIET_GIO_HANGController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CHI_TIET_GIO_HANG
        public async Task<IActionResult> Index()
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
             if (userID != null)
            {
                NGUOI_DUNG? KhachHang = _context.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(userID) && x.PhanQuyen == false && x.TrangThai == true).FirstOrDefault();
                if (KhachHang == null)
                {
                    TempData["DangNhap_User"] = "Vui lòng đăng nhập";

                    return View();
                }
                else
                {
                    var gioHang = _context.db_GIO_HANG.Where(x => x.MaNguoiDung == KhachHang.MaND).ToList();
                    var gioHangChiTiet = gioHang.FirstOrDefault(x => !_context.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang));

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemVaoGioHang(int id, int quantity)
        {
            if(quantity < 1)
            {
                TempData["SoLuongSP"] = "Số lượng sản phẩm phải lớn hơn 0";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                // Kiểm tra mã giỏ hàng đã tồn tại trong hóa đơn hay chưa
                var gioHang = _context.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(userID)).ToList(); // Trả về giỏ hàng của người dùng 
                // Lấy ra giỏ hàng của người dùng không có trong hóa đơn
                var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_context.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang));
                if (gioHangChuaCoTrongHoaDon == null) // nếu không có thì tạo mới
                {
                    // Tạo giỏ hàng mới
                    GIO_HANG newGioHang = new GIO_HANG();
                    newGioHang.MaNguoiDung = int.Parse(userID);
                    _context.db_GIO_HANG.Add(newGioHang);
                    _context.SaveChanges();
                    // Lấy ra giỏ hàng mới tạo
                    gioHangChuaCoTrongHoaDon = _context.db_GIO_HANG.FirstOrDefault(x => x.MaNguoiDung == int.Parse(userID) && !_context.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang));
                    Console.WriteLine("Mã Giỏ Hàng : ");
                    Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
                }

                // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
                var chiTietGioHang = _context.db_CHI_TIET_GIO_HANG.FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang && x.MaSP == id);
                SAN_PHAM? sp = _context.db_SAN_PHAM.Where(x => x.MaSP == id && x.TrangThai == true).FirstOrDefault();


                // Nếu sản phẩm đã có trong giỏ hàng thì cập nhật số lượng
                if (chiTietGioHang != null)
                {
                    chiTietGioHang.SoLuong += quantity;

                    _context.SaveChanges();
                    //kiểm tra số lượng sản phẩm có đủ không, nếu đủ thì cho phép thêm vào giò, nếu không thì thông báo vào tempdata


                }
                else if(sp != null)
                {
                    // Nếu sản phẩm chưa có trong giỏ hàng thì thêm mới
                    CHI_TIET_GIO_HANG newChiTietGioHang = new CHI_TIET_GIO_HANG();
                    newChiTietGioHang.MaGioHang = gioHangChuaCoTrongHoaDon.MaGioHang;
                    newChiTietGioHang.MaSP = id;
                    newChiTietGioHang.SoLuong = quantity;
                    _context.db_CHI_TIET_GIO_HANG.Add(newChiTietGioHang);
                    _context.SaveChanges();
                    TempData["tbThemVaoGioHang"] = "Thêm vào giỏ hàng thành công!";
                   
                }
                else
                {
                    TempData["tbThemVaoGioHang_ThatBai"] = "Sản phẩm không tồn tại hoặc đã ngừng bán!";
                }
            }
            else
            {
                TempData["DangNhap_User"] = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng";
                return Redirect(Request.Headers["Referer"].ToString());

            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
        // GET: CHI_TIET_GIO_HANG/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cHI_TIET_GIO_HANG = await _context.db_CHI_TIET_GIO_HANG
                .Include(c => c.GIO_HANG)
                .Include(c => c.SAN_PHAM)
                .FirstOrDefaultAsync(m => m.MaCTGH == id);
            if (cHI_TIET_GIO_HANG == null)
            {
                return NotFound();
            }

            return View(cHI_TIET_GIO_HANG);
        }

        // GET: CHI_TIET_GIO_HANG/Create
        public IActionResult Create()
        {
            ViewData["MaGioHang"] = new SelectList(_context.db_GIO_HANG, "MaGioHang", "MaGioHang");
            ViewData["MaSP"] = new SelectList(_context.db_SAN_PHAM, "MaSP", "MoTa");
            return View();
        }

        // POST: CHI_TIET_GIO_HANG/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCTGH,MaSP,SoLuong,MaGioHang")] CHI_TIET_GIO_HANG cHI_TIET_GIO_HANG)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cHI_TIET_GIO_HANG);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGioHang"] = new SelectList(_context.db_GIO_HANG, "MaGioHang", "MaGioHang", cHI_TIET_GIO_HANG.MaGioHang);
            ViewData["MaSP"] = new SelectList(_context.db_SAN_PHAM, "MaSP", "MoTa", cHI_TIET_GIO_HANG.MaSP);
            return View(cHI_TIET_GIO_HANG);
        }

        // GET: CHI_TIET_GIO_HANG/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cHI_TIET_GIO_HANG = await _context.db_CHI_TIET_GIO_HANG.FindAsync(id);
            if (cHI_TIET_GIO_HANG == null)
            {
                return NotFound();
            }
            ViewData["MaGioHang"] = new SelectList(_context.db_GIO_HANG, "MaGioHang", "MaGioHang", cHI_TIET_GIO_HANG.MaGioHang);
            ViewData["MaSP"] = new SelectList(_context.db_SAN_PHAM, "MaSP", "MoTa", cHI_TIET_GIO_HANG.MaSP);
            return View(cHI_TIET_GIO_HANG);
        }

        // POST: CHI_TIET_GIO_HANG/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCTGH,MaSP,SoLuong,MaGioHang")] CHI_TIET_GIO_HANG cHI_TIET_GIO_HANG)
        {
            if (id != cHI_TIET_GIO_HANG.MaCTGH)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cHI_TIET_GIO_HANG);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CHI_TIET_GIO_HANGExists(cHI_TIET_GIO_HANG.MaCTGH))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGioHang"] = new SelectList(_context.db_GIO_HANG, "MaGioHang", "MaGioHang", cHI_TIET_GIO_HANG.MaGioHang);
            ViewData["MaSP"] = new SelectList(_context.db_SAN_PHAM, "MaSP", "MoTa", cHI_TIET_GIO_HANG.MaSP);
            return View(cHI_TIET_GIO_HANG);
        }

        public class DelParams
        {
            public int itemId { get; set; }
        }

        // GET: CHI_TIET_GIO_HANG/Delete
        [HttpPost]
        [Route("CHI_TIET_GIO_HANG/Delete")]
        public async Task<IActionResult> Delete([FromForm] int itemId)
        {
            // tìm giò hàng của người dùng đang sử dụng
            var cookie = Request.Cookies["UserID"];
            var gioHang = _context.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList(); // Trả về giỏ hàng của người dùng
            var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_context.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang)); // Trả về giỏ hàng của người dùng chưa có trong hóa đơn
            Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
            // Tìm chi tiết giỏ hàng
            var chiTietGioHang = _context.db_CHI_TIET_GIO_HANG.FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang && x.MaSP == itemId);
            // Xóa sản phẩm khỏi giỏ hàng
            _context.db_CHI_TIET_GIO_HANG.Remove(chiTietGioHang);
            _context.SaveChanges();
            TempData["XoaSPGH"] = "Xóa sản phẩm trong giỏ hàng thành công";
            return Json(new { success = true, message = "Sản phẩm đã được xóa khỏi giỏ hàng." });
        }

        // POST: CHI_TIET_GIO_HANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int itemId)
        {
            var cookie = Request.Cookies["UserID"];
            var gioHang = _context.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList(); // Trả về giỏ hàng của người dùng
            var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_context.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang)); // Trả về giỏ hàng của người dùng chưa có trong hóa đơn
            Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
            // Tìm chi tiết giỏ hàng
            var chiTietGioHang = _context.db_CHI_TIET_GIO_HANG.FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang && x.MaSP == itemId);
            // Xóa sản phẩm khỏi giỏ hàng
            _context.db_CHI_TIET_GIO_HANG.Remove(chiTietGioHang);
            _context.SaveChanges();
            TempData["XoaSPGH"] = "Xóa sản phẩm trong giỏ hàng thành công";
            return Json(new { success = true, message = "Sản phẩm đã được xóa khỏi giỏ hàng." });
            /*var cHI_TIET_GIO_HANG = await _context.db_CHI_TIET_GIO_HANG.FindAsync(id);
            if (cHI_TIET_GIO_HANG != null)
            {
                _context.db_CHI_TIET_GIO_HANG.Remove(cHI_TIET_GIO_HANG);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/
        }

        [HttpPost]
        [Route("/CHI_TIET_GIO_HANG/UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(int itemId, int newQuantity)
        {
            //tìm giò hàng của người dùng đang sử dụng
            var cookie = Request.Cookies["UserID"];
            var gioHang = _context.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList(); // Trả về giỏ hàng của người dùng
            var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_context.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang)); // Trả về giỏ hàng của người dùng chưa có trong hóa đơn
          
            // Tìm chi tiết giỏ hàng

            var chiTietGioHang = _context.db_CHI_TIET_GIO_HANG.FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang && x.MaSP == itemId);
            SAN_PHAM sp = _context.db_SAN_PHAM.Where(x => x.MaSP == itemId && x.TrangThai == true).FirstOrDefault();
            // Cập nhật số lượng
            if (newQuantity < 1)
            {
                //thông báo temdata 
                TempData["SoLuongSP"] = "Số lượng sản phẩm phải lớn hơn 0";
                return Json(new { success = true, message = "" });
            }
            else
            {
                if (sp != null)
                {
                    chiTietGioHang.SoLuong = newQuantity;
                    _context.SaveChanges();
                    // Sau khi cập nhật, bạn có thể trả về một phản hồi, ví dụ:
                    //reload lại trang

                    return Json(new { success = true, message = "Số lượng đã được cập nhật thành công." });
                   
                }
                else
                {

                    return Json(new { error = true, message = "" });
                }
            }

        }
        private bool CHI_TIET_GIO_HANGExists(int id)
        {
            return _context.db_CHI_TIET_GIO_HANG.Any(e => e.MaCTGH == id);
        }

        /*  public ActionResult DatHang(string? ghiChu)
          {
              //tìm giò hàng của người dùng đang sử dụng
              var cookie = Request.Cookies["UserID"];
              var gioHang = _context.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList(); // Trả về giỏ hàng của người dùng
              var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_context.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang)); // Trả về giỏ hàng của người dùng chưa có trong hóa đơn
              if (gioHangChuaCoTrongHoaDon == null)
              {
                  TempData["tbDatHangLoi"] = "Không có sản phẩm trong giò hàng";
                  return RedirectToAction("ChiTietGioHang", "GioHang");
              }
              else
              {
                  // kiểm tra giỏ hàng có sản phẩm không
                  var chiTietGioHang = _context.db_CHI_TIET_GIO_HANG.FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang);
                  if (chiTietGioHang == null)
                  {
                      TempData["tbDatHangLoi"] = "Không có sản phẩm trong giò hàng";
                      return RedirectToAction("ChiTietGioHang", "GioHang");
                  }
                  else
                  {
                      //kiểm tra địa chỉ người dùng
                      var nguoiDung = _context.db_NGUOI_DUNG.FirstOrDefault(x => x.MaND == int.Parse(cookie));
                      if (nguoiDung.DiaChi == null)
                      {
                          TempData["tbDatHangLoi"] = "Vui lòng cập nhật địa chỉ trước khi đặt hàng";
                          return RedirectToAction("User_Information", "NGUOI_DUNG");
                      }
                      else
                      {

                          //tạo hóa đơn
                          HOA_DON hoaDon = new HOA_DON();
                          hoaDon.MaGioHang = gioHangChuaCoTrongHoaDon.MaGioHang;
                          hoaDon.NgayTao = DateTime.Now;
                          hoaDon.TrangThai = 0;
                          hoaDon.GhiChu = ghiChu;



                          _context.db_HOA_DON.Add(hoaDon);
                          _context.SaveChanges();
                          TempData["tbDatHang"] = "Đặt hàng thành công!";
                      }
                  }
              }
              return RedirectToAction("Index", "USER_SAN_PHAM");

          }
  */

        [HttpPost]
        public ActionResult DatHang(string? ghiChu)
        {
            var cookie = Request.Cookies["UserID"];
            if (cookie != null)
            {
                var gioHang = _context.db_GIO_HANG
                    .Where(x => x.MaNguoiDung == int.Parse(cookie))
                    .ToList();

                var gioHangChuaCoTrongHoaDon = gioHang
                    .FirstOrDefault(x => !_context.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang));

                if (gioHangChuaCoTrongHoaDon == null)
                {
                    TempData["tbDatHangLoi"] = "Không có sản phẩm trong giỏ hàng";
                    return RedirectToAction("ChiTietGioHang", "GioHang");
                }

                var chiTietGioHang = _context.db_CHI_TIET_GIO_HANG
                    .FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang);

                if (chiTietGioHang == null)
                {
                    TempData["tbDatHangLoi"] = "Không có sản phẩm trong giỏ hàng";
                    return RedirectToAction("ChiTietGioHang", "GioHang");
                }

                var nguoiDung = _context.db_NGUOI_DUNG
                    .FirstOrDefault(x => x.MaND == int.Parse(cookie));

                if (nguoiDung?.DiaChi == null)
                {
                    TempData["tbDatHangLoi"] = "Vui lòng cập nhật địa chỉ trước khi đặt hàng";
                    return RedirectToAction("User_Information", "NGUOI_DUNG");
                }

                var hoaDon = new HOA_DON
                {
                    MaGioHang = gioHangChuaCoTrongHoaDon.MaGioHang,
                    NgayTao = DateTime.Now,
                    TrangThai = 0,
                    GhiChu = ghiChu
                };

                _context.db_HOA_DON.Add(hoaDon);
                _context.SaveChanges();

                TempData["tbDatHang"] = "Đặt hàng thành công!";
                return RedirectToAction("Index", "USER_SAN_PHAM");
            }
            else
            {
                TempData["DangNhap_User"] = "Vui lòng đăng nhập để vào giỏ hàng";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

    }
}
