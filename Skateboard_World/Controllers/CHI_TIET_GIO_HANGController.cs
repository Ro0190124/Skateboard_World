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
            var applicationDbContext = _context.db_CHI_TIET_GIO_HANG.Include(c => c.GIO_HANG).Include(c => c.SAN_PHAM).ThenInclude(x=> x.DS_HINH_ANH);
            return View(await applicationDbContext.ToListAsync());
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

        // GET: CHI_TIET_GIO_HANG/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: CHI_TIET_GIO_HANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cHI_TIET_GIO_HANG = await _context.db_CHI_TIET_GIO_HANG.FindAsync(id);
            if (cHI_TIET_GIO_HANG != null)
            {
                _context.db_CHI_TIET_GIO_HANG.Remove(cHI_TIET_GIO_HANG);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CHI_TIET_GIO_HANGExists(int id)
        {
            return _context.db_CHI_TIET_GIO_HANG.Any(e => e.MaCTGH == id);
        }
    }
}
