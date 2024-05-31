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
        public IActionResult Index( NGUOI_DUNG nguoidung)
        {
            NGUOI_DUNG? user = _context.db_NGUOI_DUNG.Where(x=> x.TenTaiKhoan == nguoidung.TenTaiKhoan && x.MatKhau == nguoidung.MatKhau).FirstOrDefault();
            
            if(user == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }
            else if (user.TrangThai == true)
            {
                if (user.PhanQuyen == true)
                {
                    return RedirectToAction("Index", "Home");
                }
                if(user.PhanQuyen == false)
                {
                    return RedirectToAction("Index", "USER_SAN_PHAM");
                }

            }
            else
            {
                
            }
            return View();

        }/*

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
    }
}
