using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class TransactionDetail
    {
        [Key]
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}