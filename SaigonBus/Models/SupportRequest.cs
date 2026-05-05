using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaigonBus.Models
{
    public class SupportRequest
    {
        [Key]
        public int RequestId { get; set; }

        public int? UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
