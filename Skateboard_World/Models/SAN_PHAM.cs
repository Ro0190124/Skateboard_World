using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Skateboard_World.Models
{
    public class SAN_PHAM
    {
        [Key]
        [Display(Name ="Mã Sản Phẩm")]
        public int MaSP { get; set; }
        [MaxLength(100, ErrorMessage = "Tên sản phẩm không quá 100 kí tự")]
        [MinLength(5, ErrorMessage = "Tên sản phẩm không dưới 5 kí tự")]
        [Required(ErrorMessage = "Tên sản phẩm Không được trống")]

        [Display(Name = "Tên Sản Phẩm")]
        public string TenSP { get; set; }

        [Range(0.01, 5000000, ErrorMessage = "Giá phải lớn hơn 0 và không vượt quá 5 triệu")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Giá chỉ chấp nhận kí tự số")]
        [Display(Name = "Giá Nhập")]
        public double? GiaNhap { get; set; }

        [Range(0.01, 5000000, ErrorMessage = "Giá phải lớn hơn 0 và không vượt quá 5 triệu")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Giá chỉ chấp nhận kí tự số")]
        [Required(ErrorMessage = "Giá không được trống")]
        [Display(Name = "Giá Bán")]
        public double GiaBan { get; set; }

        [Range(1, 100, ErrorMessage = "Số lượng phải lớn hơn 0 và không vượt quá 100")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Số lượng chỉ chấp nhận kí tự số")]
        [Required(ErrorMessage = "Số lượng không được trống")]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        [Display(Name = "Ngày Tạo")]
        public DateTime NgayTao { get; set; } = DateTime.Now;
        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; } = true;

        [Required(ErrorMessage = "Mô tả Không được trống")]
        [MaxLength(5000, ErrorMessage = "Mô tả không quá 5000 kí tự")]
        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; }
        public ICollection<DS_HINH_ANH>? DS_HINH_ANH { get; set; }
    }
}
