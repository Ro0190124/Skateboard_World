﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Skateboard_World.Models
{
    public class GIO_HANG
    {
        [Key]
        public int MaGioHang { get; set; }
        public int MaNguoiDung { get; set; }
        [ForeignKey("MaNguoiDung")]
        public NGUOI_DUNG? NGUOI_DUNG { get; set; }
    }
}

