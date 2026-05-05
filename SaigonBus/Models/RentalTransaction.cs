using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaigonBus.Models
{
    public class RentalTransaction
    {
        [Key]
        [Display(Name = "Mã Giao Dịch")]
        public int TransactionId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ID khách hàng")]
        [Display(Name = "Mã Khách Hàng")]
        public int UserId { get; set; } 

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn xe")]
        [Display(Name = "Mã Phương tiện")]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạm xuất phát")]
        [Display(Name = "Trạm nhận xe")]
        public int StartStationId { get; set; }

        [Display(Name = "Trạm trả xe")]
        public int? EndStationId { get; set; }

        [Required]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime StartTime { get; set; }

        // EndTime cho phép Null vì lúc mới thuê xe thì chưa có thời gian trả
        [Display(Name = "Thời gian kết thúc")]
        public DateTime? EndTime { get; set; }

        // EF sẽ tự động map các biến này thành decimal(18,2) trong SQL Server
        [Display(Name = "Cước phí gốc")]
        public decimal? CalculatedFare { get; set; }

        [Display(Name = "Mức giảm giá")]
        public decimal? AppliedDiscount { get; set; }

        [Display(Name = "Tổng thanh toán")]
        public decimal? TotalAmount { get; set; }

        // Liên kết với GroupRental (nếu là thuê nhóm)
        public int? GroupRentalId { get; set; }

        [ForeignKey("GroupRentalId")]
        public virtual GroupRental GroupRental { get; set; }
    }
}