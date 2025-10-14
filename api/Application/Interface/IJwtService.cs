namespace api.Application.Interface
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string email);
    }
}