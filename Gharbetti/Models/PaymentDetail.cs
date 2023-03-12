using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class PayementDetail
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int TransactionId { get; set; }


        [Required]
        public int PaymentModeId { get; set; }

        [Required]
        public string? Remarks { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }
        public string TransactionNumber { get; set; }

    }
}