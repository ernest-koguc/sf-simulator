namespace SFSimulator.Core;

public interface IRuneValueProvider
{
    int GetRuneValue(RuneType runeType, int runesQuantity);
}