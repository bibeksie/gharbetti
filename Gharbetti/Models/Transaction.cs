using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public decimal Total { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Remarks { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RentAmount { get; set; }
        public decimal RentPaid { get; set; }
        public int PaymentModeId { get; set; }  

    }
}