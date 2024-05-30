namespace Skateboard_World.Models
{
    public class HINH_ANH_SAN_PHAM
    {
        public SAN_PHAM SanPham { get; set; }
        public List<DS_HINH_ANH> HinhAnhList { get; set; }
        public List<IFormFile> NewImages { get; set; }
    }
}
