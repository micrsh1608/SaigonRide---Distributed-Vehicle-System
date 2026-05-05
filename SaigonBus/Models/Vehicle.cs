using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SaigonBus.Models
{
    public enum VehicleCategory { StandardBike, EBike, Scooter }

    // Trạng thái xe
    public enum VehicleStatus { Available, InTransit, Maintenance }

    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        public VehicleCategory Category { get; set; }

        [Required]
        public VehicleStatus Status { get; set; }

        // Khóa ngoại liên kết với Station (Có thể null nếu xe đang di chuyển, không ở trạm nào)
        public int? CurrentStationId { get; set; }

        [ForeignKey("CurrentStationId")]
        public virtual Station CurrentStation { get; set; }

        // Logic giữ chỗ 15p
        public DateTime? ReservedUntil { get; set; }
        public int? ReservedByUserId { get; set; }

        [ForeignKey("ReservedByUserId")]
        public virtual User ReservedByUser { get; set; }
    }
}