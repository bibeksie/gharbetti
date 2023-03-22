using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}