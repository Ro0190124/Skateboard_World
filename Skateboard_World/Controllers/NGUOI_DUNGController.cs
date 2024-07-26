using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skateboard_World.Data;
using Skateboard_World.Models;

namespace Skateboard_World.Controllers
{
    public class NGUOI_DUNGController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NGUOI_DUNGController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NGUOI_DUNG
        public async Task<IActionResult> Index(string? search)
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                if(search != null)
                {
                    return View(await _context.db_NGUOI_DUNG
                        .Where(
                        x => x.TenND.Contains(search) || 
                        x.TenTaiKhoan.Contains(search) ||
                        x.SoDienThoai.Contains(search) ||
                        x.Email.Contains(search) &&
                        x.TrangThai == true && 
                        x.PhanQuyen == true).ToListAsync());
                }
                else
                {
                    return View(await _context.db_NGUOI_DUNG.Where(x => x.TrangThai == true && x.PhanQuyen == true).ToListAsync());

                }

            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
          
        }

        // GET: NGUOI_DUNG/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {

                if (id == null)
                {
                    return NotFound();
                }

                var nGUOI_DUNG = await _context.db_NGUOI_DUNG
                    .FirstOrDefaultAsync(m => m.MaND == id);
                if (nGUOI_DUNG == null)
                {
                    return NotFound();
                }

                return View(nGUOI_DUNG);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }

        // GET: NGUOI_DUNG/Create
        public IActionResult Create()
        {

            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
           
        }

        // POST: NGUOI_DUNG/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaND,TenND,SoDienThoai,Email,TenTaiKhoan,MatKhau,NgaySinh,NgayTao,DiaChi,TrangThai")] NGUOI_DUNG nGUOI_DUNG)
        {
            NGUOI_DUNG? check = _context.db_NGUOI_DUNG.Where(x => x.TenTaiKhoan == nGUOI_DUNG.TenTaiKhoan && x.TrangThai == true).FirstOrDefault();
            if (check != null)
            {

                TempData["create_Fail"] = "Tên tài khoản đã tồn tại";
                return View();
            }
            NGUOI_DUNG? check_SDT = _context.db_NGUOI_DUNG.Where(x => x.SoDienThoai == nGUOI_DUNG.SoDienThoai && x.TrangThai == true).FirstOrDefault();
            if (check == null && check_SDT != null)
            {
                TempData["tb_DangKi_KhongThanhCong"] = "Số Điện Thoại đã tồn tại";
                return View();
            }

            if (ModelState.IsValid)
            {
                
                nGUOI_DUNG.PhanQuyen = true;
                if(nGUOI_DUNG.NgaySinh == null)
                {
                    nGUOI_DUNG.NgaySinh = new DateTime(2001, 01, 01);
                }
                _context.Add(nGUOI_DUNG);
                TempData["create_Success"] = "Thêm người dùng thành công";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nGUOI_DUNG);
        }

        // GET: NGUOI_DUNG/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {

                if (id == null)
                {
                    return NotFound();
                }

                var nGUOI_DUNG = await _context.db_NGUOI_DUNG.FindAsync(id);
                if (nGUOI_DUNG == null)
                {
                    return NotFound();
                }
                return View(nGUOI_DUNG);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }

        // POST: NGUOI_DUNG/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaND,TenND,SoDienThoai,Email,TenTaiKhoan,MatKhau,NgaySinh,NgayTao,DiaChi,PhanQuyen,TrangThai")] NGUOI_DUNG nGUOI_DUNG)
        {
            if (id != nGUOI_DUNG.MaND)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    nGUOI_DUNG.PhanQuyen = true;
                    _context.Update(nGUOI_DUNG);
                    TempData["create_Success"] = "Sửa người dùng thành công";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NGUOI_DUNGExists(nGUOI_DUNG.MaND))
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
            return View(nGUOI_DUNG);
        }

        // GET: NGUOI_DUNG/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var nGUOI_DUNG = await _context.db_NGUOI_DUNG
                    .FirstOrDefaultAsync(m => m.MaND == id);
                if (nGUOI_DUNG == null)
                {
                    return NotFound();
                }

                return View(nGUOI_DUNG);

            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }

        // POST: NGUOI_DUNG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nGUOI_DUNG = await _context.db_NGUOI_DUNG.FindAsync(id);
            if (nGUOI_DUNG != null)
            {
               nGUOI_DUNG.TrangThai = false;
                _context.Update(nGUOI_DUNG);
                TempData["delete_Success"] = "Xóa người dùng thành công";   
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NGUOI_DUNGExists(int id)
        {
            return _context.db_NGUOI_DUNG.Any(e => e.MaND == id);
        }



        public async Task<IActionResult> User_Information()
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {

                if (userID == null)
                {
                    return NotFound();
                }

                var nGUOI_DUNG = await _context.db_NGUOI_DUNG.FindAsync(int.Parse(userID));

                if (nGUOI_DUNG == null)
                {
                    return NotFound();
                }
                return View(nGUOI_DUNG);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }

        // POST: NGUOI_DUNG/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> User_Information(int id,[Bind("MaND,TenND,SoDienThoai,Email,TenTaiKhoan,MatKhau,NgaySinh,DiaChi,PhanQuyen,TrangThai")] NGUOI_DUNG nGUOI_DUNG)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nGUOI_DUNG);
                    await _context.SaveChangesAsync();
                    TempData["update_Success"] = "Cập nhật thông tin thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NGUOI_DUNGExists(nGUOI_DUNG.MaND))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {

                TempData["update_Fail"] = "Cập nhật thông tin thất bại";
            }
            return View(nGUOI_DUNG);
        }
    }
}
