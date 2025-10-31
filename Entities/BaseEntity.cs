using SecurityDriven;

namespace api.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = FastGuid.NewGuid();
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}