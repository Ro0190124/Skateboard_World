﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skateboard_World.Data;
using Skateboard_World.Models;

namespace Skateboard_World.Controllers
{
    public class USER_SAN_PHAM : Controller
    {
        private readonly ApplicationDbContext _context;

        public USER_SAN_PHAM(ApplicationDbContext context)
        {
            _context = context;
        }

        //[Route("/UserIndex")]
        public IActionResult Index()
        {
            var products = _context.db_SAN_PHAM.Where(x => x.TrangThai == true).ToList();

            var productWithImages = products.Select(p => new HINH_ANH_SAN_PHAM
            {
                SanPham = p,
                HinhAnhList = _context.db_DS_HINH_ANH.Where(h => h.MaSP == p.MaSP).ToList()
            }).ToList();
          

            var tatcachitiet = _context.db_CHI_TIET_GIO_HANG.ToList();
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
            var sanPhamNoiBat = sanpham_soluong.OrderByDescending(x => x.Value).Take(3)
                .Select(x => new
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

            return View(productWithImages);
        }

        public async Task<IActionResult> Details(int? id)
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
            var danhSachSP = _context.db_SAN_PHAM
          .Where(x => x.TrangThai == true)
          .Include(x => x.DS_HINH_ANH) // Include related images if needed
          .ToList();

            ViewData["DanhSachSP"] = danhSachSP;
            Console.WriteLine("Danh sach hinh anh: " + sAN_PHAM.DS_HINH_ANH.Count());


            return View(sAN_PHAM);
        }
        public IActionResult Product(string sortOrder, string searchString)
        {
            var products = _context.db_SAN_PHAM.Where(x => x.TrangThai == true).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.TenSP.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.TenSP);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.TenSP);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.GiaBan);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.GiaBan);
                    break;
                default:
                    break;
            }

            var productWithImages = products.Select(p => new HINH_ANH_SAN_PHAM
            {
                SanPham = p,
                HinhAnhList = _context.db_DS_HINH_ANH.Where(h => h.MaSP == p.MaSP).ToList()
            }).ToList();

            return View(productWithImages);
        }

    }
}
