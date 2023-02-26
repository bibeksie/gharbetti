using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class HouseRoom
    {
        [Key]
        public int Id { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
    }
}