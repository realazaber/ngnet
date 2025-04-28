using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
