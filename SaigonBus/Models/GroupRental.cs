using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaigonBus.Models
{
    public class GroupRental
    {
        [Key]
        public int GroupRentalId { get; set; }

        public int UserId { get; set; }

        public int StartStationId { get; set; }

        public int? EndStationId { get; set; }

        public int Quantity { get; set; }

        [StringLength(50)]
        public string VehicleCategory { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active";

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("StartStationId")]
        public virtual Station StartStation { get; set; }

        [ForeignKey("EndStationId")]
        public virtual Station EndStation { get; set; }
    }
}
