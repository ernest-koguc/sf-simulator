﻿using SFSimulator.Core;

namespace SFSimulator.Frontend;

public class SavedResultEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateOnly Started { get; set; }
    public required DateOnly Finished { get; set; }
    public DateTime LastModification { get; set; }
    public required int Days { get; set; }
    public required ContextSnapshot BeforeSimulation { get; set; }
    public required ContextSnapshot AfterSimulation { get; set; }
    public required List<SimulatedGains> SimulatedDays { get; set; }
    public required List<SimulationAchievement> Achievements { get; set; }
}

