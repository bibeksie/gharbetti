using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class TenantMessage
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public Guid TenantId { get; set; }
        public byte Status { get; set; }
                
    }
}