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
    public class HOA_DON_ADMINController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HOA_DON_ADMINController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HOA_DON_ADMIN
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.db_HOA_DON.Include(h => h.GIO_HANG);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HOA_DON_ADMIN/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOA_DON = await _context.db_HOA_DON
                .Include(h => h.GIO_HANG)
                .FirstOrDefaultAsync(m => m.MaHD == id);
            if (hOA_DON == null)
            {
                return NotFound();
            }

            return View(hOA_DON);
        }

        // GET: HOA_DON_ADMIN/Create
        public IActionResult Create()
        {
            ViewData["MaGioHang"] = new SelectList(_context.db_GIO_HANG, "MaGioHang", "MaGioHang");
            return View();
        }

        // POST: HOA_DON_ADMIN/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHD,NgayTao,TrangThai,GhiChu,MaGioHang")] HOA_DON hOA_DON)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hOA_DON);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGioHang"] = new SelectList(_context.db_GIO_HANG, "MaGioHang", "MaGioHang", hOA_DON.MaGioHang);
            return View(hOA_DON);
        }

        // GET: HOA_DON_ADMIN/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOA_DON = await _context.db_HOA_DON.FindAsync(id);
            if (hOA_DON == null)
            {
                return NotFound();
            }
            ViewData["MaGioHang"] = new SelectList(_context.db_GIO_HANG, "MaGioHang", "MaGioHang", hOA_DON.MaGioHang);
            return View(hOA_DON);
        }

        // POST: HOA_DON_ADMIN/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHD,NgayTao,TrangThai,GhiChu,MaGioHang")] HOA_DON hOA_DON)
        {
            if (id != hOA_DON.MaHD)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hOA_DON);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HOA_DONExists(hOA_DON.MaHD))
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
            ViewData["MaGioHang"] = new SelectList(_context.db_GIO_HANG, "MaGioHang", "MaGioHang", hOA_DON.MaGioHang);
            return View(hOA_DON);
        }

        // GET: HOA_DON_ADMIN/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOA_DON = await _context.db_HOA_DON
                .Include(h => h.GIO_HANG)
                .FirstOrDefaultAsync(m => m.MaHD == id);
            if (hOA_DON == null)
            {
                return NotFound();
            }

            return View(hOA_DON);
        }

        // POST: HOA_DON_ADMIN/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hOA_DON = await _context.db_HOA_DON.FindAsync(id);
            if (hOA_DON != null)
            {
                _context.db_HOA_DON.Remove(hOA_DON);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HOA_DONExists(int id)
        {
            return _context.db_HOA_DON.Any(e => e.MaHD == id);
        }
    }
}
