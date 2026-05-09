using SaigonBus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SaigonBus.Controllers
{
    [Table("PointTransactions")]
    public class PointTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int UserId { get; set; }

        public int Points { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}