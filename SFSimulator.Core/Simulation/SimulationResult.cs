namespace SFSimulator.Core
{
    public class SimulationResult
    {
        public int Days { get; set; }

        public CharacterDTO CharacterPreviously { get; set; } = null!;
        public CharacterDTO CharacterAfter { get; set; } = null!;

        public List<SimulatedDay> SimulatedDays { get; set; } = null!;
    }
}
