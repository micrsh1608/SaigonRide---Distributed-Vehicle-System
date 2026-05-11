using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaigonBus.Models
{
    public class User
    {
        [Key]
        [Display(Name = "Mã Khách Hàng")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(50)]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Display(Name = "Số CCCD/CMND")]
        [StringLength(20)]
        public string CCCD { get; set; }

        [Display(Name = "Số Hộ chiếu")]
        [StringLength(50)]
        public string Passport { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Số dư ví")]
        public decimal Balance { get; set; } = 0;

        public int Point { get; set; }
    }
}
