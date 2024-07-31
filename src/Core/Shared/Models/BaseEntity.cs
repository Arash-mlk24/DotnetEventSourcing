namespace DotnetEventSourcing.src.Core.Shared.Models;

public abstract class BaseEntity
{
    public string Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? ModifiedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public bool IsVisible { get; protected set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        IsDeleted = false;
        IsVisible = true;
    }
}