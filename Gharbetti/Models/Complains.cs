using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class Complain
    {
        [Key]
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public string Reason { get; set; }
        public string? Response { get; set; }
        public DateTime ComplainDate { get; set; }
        public byte Status { get; set; }
    }
}