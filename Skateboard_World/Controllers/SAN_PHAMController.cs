using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skateboard_World.Data;
using Skateboard_World.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Skateboard_World.Controllers
{
    public class SAN_PHAMController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SAN_PHAMController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: SAN_PHAM
        public async Task<IActionResult> Index(string? search)
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                var products = await _context.db_SAN_PHAM.ToListAsync();
                if (search == null)
                {
                     products = await _context.db_SAN_PHAM.Where(x => x.TrangThai == true).ToListAsync();
                }
                else
                {
                    products = await _context.db_SAN_PHAM.Where(x => x.TenSP.Contains(search) && x.TrangThai == true).ToListAsync();
                }

                var productWithImages = products.Select(p => new HINH_ANH_SAN_PHAM
                {
                    SanPham = p,
                    HinhAnhList = _context.db_DS_HINH_ANH.Where(h => h.MaSP == p.MaSP).ToList()
                }).ToList();

                return View(productWithImages);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
           
        }

        // GET: SAN_PHAM/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {

                if (id == null)
                {
                    return NotFound();
                }

                var sAN_PHAM = await _context.db_SAN_PHAM
                    .Include(s => s.DS_HINH_ANH)
                    .FirstOrDefaultAsync(m => m.MaSP == id);
                if (sAN_PHAM == null)
                {
                    return NotFound();
                }

                return View(sAN_PHAM);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
           
        }

        // GET: SAN_PHAM/Create
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

        // POST: SAN_PHAM/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSP,TenSP,GiaNhap,GiaBan,SoLuong,MoTa")] SAN_PHAM sAN_PHAM, List<IFormFile> HinhAnhTaiLen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sAN_PHAM);
                await _context.SaveChangesAsync();

                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                if (HinhAnhTaiLen != null && HinhAnhTaiLen.Count > 0)
                {
                    foreach (var file in HinhAnhTaiLen)
                    {
                        if (file.Length > 0)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var hinhAnh = new DS_HINH_ANH
                            {
                                MaSP = sAN_PHAM.MaSP,
                                MediaHinhAnh = "/images/" + uniqueFileName
                            };
                            _context.Add(hinhAnh);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["update_Success"] = "Thêm sản phẩm thành công";
                }
                else
                {
                    TempData["update_Fail"] = "Thêm sản phẩm thất bại";
                }

                return RedirectToAction(nameof(Index));
            }

            // Log ModelState errors
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(sAN_PHAM);
        }

        // GET: SAN_PHAM/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID != null)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var sAN_PHAM = await _context.db_SAN_PHAM
                    .Include(s => s.DS_HINH_ANH)
                    .FirstOrDefaultAsync(m => m.MaSP == id);

                if (sAN_PHAM == null)
                {
                    return NotFound();
                }


                return View(sAN_PHAM);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
           
        }

        // POST: SAN_PHAM/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSP,TenSP,GiaNhap,GiaBan,SoLuong,MoTa")] SAN_PHAM sAN_PHAM, List<IFormFile> HinhAnhTaiLen, int[] selectedImages)
        {
            if (id != sAN_PHAM.MaSP)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sAN_PHAM);

                    // Handle image upload
                    if (HinhAnhTaiLen != null && HinhAnhTaiLen.Count > 0)
                    {
                        foreach (var file in HinhAnhTaiLen)
                        {
                            if (file.Length > 0)
                            {
                                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                var hinhAnh = new DS_HINH_ANH
                                {
                                    MaSP = sAN_PHAM.MaSP,
                                    MediaHinhAnh = "/images/" + uniqueFileName
                                };
                                _context.Add(hinhAnh);
                            }
                        }
                    }

                    // Xóa các hình ảnh đã chọn
                    if (selectedImages != null && selectedImages.Length > 0)
                    {
                        foreach (var imageId in selectedImages)
                        {
                            var imageToRemove = await _context.db_DS_HINH_ANH.FindAsync(imageId);
                            if (imageToRemove != null)
                            {
                                _context.db_DS_HINH_ANH.Remove(imageToRemove);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["update_Success"] = "Cập nhật sản phẩm thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SAN_PHAMExists(sAN_PHAM.MaSP))
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
            return View(sAN_PHAM);
        }
        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteImage(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var image = await _context.db_DS_HINH_ANH.FindAsync(id);
                    if (image == null)
                    {
                        return NotFound();
                    }

                    _context.db_DS_HINH_ANH.Remove(image);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Edit), new { id = image.MaSP });
                }*/

        // GET: SAN_PHAM/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            string? userID = HttpContext.Request.Cookies["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Index", "DangNhap");
            }

            if (id == null)
            {
                return NotFound();
            }

            var sAN_PHAM = await _context.db_SAN_PHAM
                .Include(s => s.DS_HINH_ANH)
                .FirstOrDefaultAsync(m => m.MaSP == id);

            if (sAN_PHAM == null)
            {
                return NotFound();
            }

            return View(sAN_PHAM);
        }

        // POST: SAN_PHAM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sAN_PHAM = await _context.db_SAN_PHAM.FindAsync(id);
            if (sAN_PHAM != null)
            {
                sAN_PHAM.TrangThai = false;
                sAN_PHAM.SoLuong = 0;
                _context.db_SAN_PHAM.Update(sAN_PHAM);
                await _context.SaveChangesAsync();
                TempData["update_Success"] = "Xóa sản phẩm thành công";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SAN_PHAMExists(int id)
        {
            return _context.db_SAN_PHAM.Any(e => e.MaSP == id);
        }
    }
}
