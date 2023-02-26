using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class RoomLeaseDetail
    {
        [Key]
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RentAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}