namespace backend.Models
{
    public abstract class AuthEntity : Entity
    {
        public Guid CreatorId { get; set; }
    }
}
