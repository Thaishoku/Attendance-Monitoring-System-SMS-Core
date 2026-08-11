using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceMonitoringSystem.Models
{
    public class SmsLogs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [Required]
        public string ReceiverNumber { get; set; } = string.Empty;

        [Required]
        public string MessageContent { get; set; } = string.Empty;

        [Required]
        public DateTime SentTimestamp { get; set; } = DateTime.Now;

        [Required]
        public string DeliveryStatus { get; set; } = "Sent";
    }
}
