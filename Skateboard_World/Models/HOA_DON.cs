using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Skateboard_World.Models
{
    public class HOA_DON
    {
        [Key]
        public int MaHD { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public byte TrangThai { get; set; } = 0;
        [MaxLength(100, ErrorMessage = "Ghi chú không lớn hơn 100 ký tự")]
        public string? GhiChu { get; set; }
        public int MaGioHang { get; set; }
        [ForeignKey("MaGioHang")]
        [ValidateNever]
        public GIO_HANG? GIO_HANG { get; set; }
    }
}
