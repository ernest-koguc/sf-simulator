namespace SFSimulator.Core;

public class Pet
{
    public required int Level { get; set; }
    public required int Position { get; set; }
    public required PetElementType ElementType { get; set; }
    public required bool CanBeObtained { get; set; }
    public required bool IsDefeated { get; set; }
    public required bool IsObtained { get; set; }
    public int LastDayWhenFed { get; set; }
    public int LastFedAmount { get; set; }
}