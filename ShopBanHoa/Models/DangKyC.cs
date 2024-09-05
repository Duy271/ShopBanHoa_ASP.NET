using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShopBanHoa.Models
{
    public class DangKyC
    {
        [Required(ErrorMessage = "Vui Lòng Không Để Trống!")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Tài Khoản Phải Trên 6 Ký Tự")]
        public string TaiKhoanC { get; set; }

        [Required(ErrorMessage = "Vui Lòng Không Để Trống!")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật Khẩu Phải Trên 6 Ký Tự")]
        public string MatKhauC { get; set; }

        [Required(ErrorMessage = "Vui Lòng Không Để Trống!")]
        [Compare("MatKhauC", ErrorMessage = "Mật Khẩu Xác Nhận Không Trùng Khớp")]
        public string XacNhanMatKhau { get; set; }
    }
}