namespace Backend.DTOs.Roles
{
    public record RoleUserCountDTO(Guid Id, string name, List<Guid> userIds);    
}
