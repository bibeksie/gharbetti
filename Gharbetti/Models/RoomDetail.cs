using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class RoomDetail
    {
        [Key]
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string? SquareFootage { get; set; }
    }
}