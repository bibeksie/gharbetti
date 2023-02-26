using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class Transacition
    {
        [Key]
        public int Id { get; set; }
        public int RoomLeaseDetailId { get; set; }
        public decimal Total  { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Remarks { get; set; }
        
    }
}