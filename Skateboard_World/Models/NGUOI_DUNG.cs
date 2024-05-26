using System.ComponentModel.DataAnnotations;

namespace Skateboard_World.Models
{
    public class NGUOI_DUNG
    {
        [Key]
        public int MaND { get; set; }

        [Required(ErrorMessage = "Không được trống")]
        [MinLength(5, ErrorMessage = "Tên người dùng không dưới 5 kí tự ")]
        [MaxLength(50, ErrorMessage = "Tên người dùng không lớn hơn 50 kí tự ")]
        [RegularExpression(@"^[\p{L}a-zA-Z\s]*$", ErrorMessage = "Chỉ cho phép nhập ký tự từ chữ.")]
        [Display(Name ="Tên Người Dùng")]
        public string TenND { get; set; }

        [StringLength(10, ErrorMessage = "Số Điện thoại phải có 10 kí tự")]
        [MinLength(10, ErrorMessage = "Số Điện thoại phải có 10 kí tự")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Số điện thoại chỉ chứa số")]
        [Required(ErrorMessage = "Không được trống")]
        [Display(Name = "Số Điện Thoại")]
        public string? SoDienThoai { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]*$", ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Không được trống")]
        [MaxLength(25)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên tài khoản chỉ được chứa ký tự và số.")]
        [MinLength(5, ErrorMessage = "Tên tài khoản không dưới 5 kí tự")]
        [Display(Name = "Tên tài khoản")]
        public string TenTaiKhoan { get; set; }



        [Required(ErrorMessage = "Không được trống")]
        [MaxLength(25, ErrorMessage = "Mật khẩu không lớn hơn 25 kí tự ")]
        [MinLength(5, ErrorMessage = "Mật khẩu không dưới 5 kí tự")]
        [Display(Name = "Mật Khẩu")]
        public string MatKhau { get; set; }
        [Display(Name = "Ngày Sinh")]
        public DateTime? NgaySinh { get; set; } = new DateTime(2001, 01, 01);
        [Display(Name = "Ngày Tạo")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        //[RegularExpression(@"^[a-zA-Z0-9., \/\(\)'""]+$", ErrorMessage = "Địa chỉ không được chứa ký tự đặc biệt.")]
        [MaxLength(150, ErrorMessage = "Địa chỉ không lớn hơn 150 kí tự ")]
        [MinLength(5, ErrorMessage = "Địa chỉ không dưới 5 kí tự")]
        [Display(Name = "Địa Chỉ")]
        public string? DiaChi { get; set; }
        public bool PhanQuyen { get; set; }//true = admin , false = nguoidung
        public bool TrangThai { get; set; } = true; //true = ton tai , false = nghi;
    }
}
