using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class DbEvent : Entity
    {
            
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public User User { get; set; }
    }
}
