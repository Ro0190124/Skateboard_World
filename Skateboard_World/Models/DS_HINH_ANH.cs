using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Skateboard_World.Models
{
    public class DS_HINH_ANH
    {
        [Key]
        public int MaDSHinhAnh { get; set; }
        public string MediaHinhAnh { get; set; }
        public int MaSP { get; set; }
        [ForeignKey("MaSP")]
        public SAN_PHAM SAN_PHAM { get; set; }
    }
}
