using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class TransactionViewModel
    {
        public TransactionViewModel() 
        {
            TransactionDetails = new List<TransactionDetailViewModel>();
        }
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
        public string? TransactionDateString { get; set; } 
        public string? StartDateString { get; set; } 
        public string? EndDateString { get; set; } 
        public int PaymentModeId { get; set; }
        public List<TransactionDetailViewModel> TransactionDetails { get; set; } 
    }
}