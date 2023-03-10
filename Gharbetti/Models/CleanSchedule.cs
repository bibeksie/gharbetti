using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class CleanSchedule
    {
        [Key]
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatedBy { get; set; }
        public string? Remarks { get; set; }
    }
}