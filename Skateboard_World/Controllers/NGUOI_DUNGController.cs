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
        public async Task<IActionResult> Index()
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                return View(await _context.db_NGUOI_DUNG.Where(x=> x.TrangThai==true && x.PhanQuyen == true).ToListAsync());
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
            if (ModelState.IsValid)
            {
                nGUOI_DUNG.PhanQuyen = true;
                _context.Add(nGUOI_DUNG);
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
                    _context.Update(nGUOI_DUNG);
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
                _context.db_NGUOI_DUNG.Remove(nGUOI_DUNG);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NGUOI_DUNGExists(int id)
        {
            return _context.db_NGUOI_DUNG.Any(e => e.MaND == id);
        }
    }
}
