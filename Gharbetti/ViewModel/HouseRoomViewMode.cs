using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class HouseRoomViewModel
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
    }
}