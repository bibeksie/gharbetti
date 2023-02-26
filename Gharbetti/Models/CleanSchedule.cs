using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class CleanSchedule
    {
        [Key]
        public int Id { get; set; }
        public int TenetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
    }
}