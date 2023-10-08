namespace SFSimulator.Core
{
    public interface ICurves
    {
        List<decimal> GoldCurve { get; }
        List<long> ExperienceCurve { get; }
    }
}