namespace SFSimulator.Frontend;

public interface IEntity<T>
{
    public T Id { get; set; }
    public DateTime LastModification { get; set; }
}