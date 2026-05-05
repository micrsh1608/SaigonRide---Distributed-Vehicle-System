using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaigonBus.Models
{
    public class Station
    {
        public Station()
        {
            // Khởi tạo danh sách xe để tránh lỗi NullReferenceException
            Vehicles = new HashSet<Vehicle>();
        }

        [Key]
        public int StationId { get; set; }

        [Required]
        [StringLength(200)]
        public string LocationName { get; set; }

        [Required]
        public int CapacityLimit { get; set; }

        public int CurrentInventoryCount { get; set; }

        // Navigation property: 1 Trạm có nhiều Xe
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}