using SFSimulator.Core;

namespace SFSimulator.Frontend;

public class SavedOptionsEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime LastModification { get; set; }
    public required SimulationContext Options { get; set; }
}
