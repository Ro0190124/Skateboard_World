using Microsoft.AspNetCore.Mvc;
using Skateboard_World.Data;
using Skateboard_World.Models;

using System.Text;

namespace Skateboard_World.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DangNhapController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index( NGUOI_DUNG nguoidung)
        {
            NGUOI_DUNG? user = _context.db_NGUOI_DUNG.Where(x=> x.TenTaiKhoan == nguoidung.TenTaiKhoan).FirstOrDefault();
            
            if(user == null)
            {
                TempData["DangNhap"] = "Tên Tài khoản không chính xác";
                return RedirectToAction("Index", "DangNhap");
            }
            else if ( user.TrangThai == true )
            {
                if(user.MatKhau == nguoidung.MatKhau)
                {
                    HttpContext.Response.Cookies.Append("UserID", user.MaND.ToString());
                    HttpContext.Response.Cookies.Append("Power", user.PhanQuyen.ToString());
                    //TempData["DangNhap_TC"] = "Đăng nhập thành công";
                    if (user.PhanQuyen == true)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.PhanQuyen == false)
                    {
                        return RedirectToAction("Index", "USER_SAN_PHAM");
                    }
                }
                else
                {
                    TempData["DangNhap"] = "Mật khẩu không chính xác";
                }
               

            }
            else
            {
                TempData["DangNhap"] = "Tài Khoản đã bị khóa";
            }
            return View();

        }
        /*

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
           *//* using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var postData = reader.ReadToEnd();
                Console.WriteLine(postData);
            }*//*
            Console.WriteLine(username);
            Console.WriteLine(password);
            return RedirectToAction("Index", "Home");
        }
*/
        [Route("/DangKi")]
        public IActionResult DangKi()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/DangKi")]
        public async Task<IActionResult> DangKi([Bind("MaND,TenND,SoDienThoai,Email,TenTaiKhoan,MatKhau,NgaySinh,NgayTao,DiaChi,TrangThai")] NGUOI_DUNG nGUOI_DUNG)
        {
            NGUOI_DUNG? check = _context.db_NGUOI_DUNG.Where(x => x.TenTaiKhoan == nGUOI_DUNG.TenTaiKhoan && x.TrangThai == true).FirstOrDefault();
            if (check != null)
            {

                TempData["tb_DangKi_KhongThanhCong"] = "Tên tài khoản đã tồn tại";
                return View();
            }
            NGUOI_DUNG? check_SDT = _context.db_NGUOI_DUNG.Where(x => x.SoDienThoai == nGUOI_DUNG.SoDienThoai && x.TrangThai == true).FirstOrDefault();
            if (check == null && check_SDT != null )
            {
                TempData["tb_DangKi_KhongThanhCong"] = "Số Điện Thoại đã tồn tại";
                return View();
            }
            if (ModelState.IsValid)
            {
                // acc khách
                nGUOI_DUNG.PhanQuyen = false;
                _context.Add(nGUOI_DUNG);
                TempData["tb_DangKi_ThanhCong"] = "Đăng kí thành công";
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                /* StringBuilder sb = new StringBuilder();
                 foreach (var modelState in ViewData.ModelState.Values)
                 {
                     foreach (var error in modelState.Errors)
                     {
                         sb.Append(error.ErrorMessage);
                     }
                 }
                 Console.WriteLine(sb.ToString());*/
                return View(nGUOI_DUNG);
            }
            
        }

    }
}
