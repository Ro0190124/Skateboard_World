using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Skateboard_World.Models
{
    public class CHI_TIET_GIO_HANG
    {
        [Key]
        public int MaCTGH { get; set; }
        public int MaSP { get; set; }
        [ForeignKey("MaSP")]
        public SAN_PHAM? SAN_PHAM { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }
        public int MaGioHang { get; set; }
        [ForeignKey("MaGioHang")]
        public GIO_HANG? GIO_HANG { get; set; }
    }
}
