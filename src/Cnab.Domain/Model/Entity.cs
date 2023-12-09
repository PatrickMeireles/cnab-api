namespace Cnab.Domain.Model;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    public Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Entity(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.Now;
    }

}
