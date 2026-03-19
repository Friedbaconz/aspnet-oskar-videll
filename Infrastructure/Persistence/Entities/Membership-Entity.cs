
namespace Infrastructure.Persistence.Entities;

public class MembershipEntity
{
    public Guid MembershipID { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Status { get; set; } = null!;

    public ICollection<UserEntity> Users = null!;

}
