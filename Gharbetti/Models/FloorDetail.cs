using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class FloorDetail
    {
        [Key]
        public int Id { get; set; }
        public int HouseId { get; set; }
        public int FloorId { get; set; }
        public string Remarks { get; set; }
    }
}