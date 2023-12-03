namespace TimeSheet.Domain.Entities;

public abstract class BaseEntity<T>
{
    public T Id { get; protected set; }

    public void SetId(T id)
    {
        Id = id;
    }
}