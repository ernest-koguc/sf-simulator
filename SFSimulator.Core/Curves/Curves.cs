namespace SFSimulator.Core
{
    public class Curves : ICurves
    {

        public List<decimal> GoldCurve { get; } = new(650) { 0, 25, 50, 75 };
        public Curves()
        {
            for (int i = GoldCurve.Count; i < 640; i++)
            {
                var result = Math.Min(Math.Floor((GoldCurve[i - 1] + Math.Floor(GoldCurve[(int)Math.Floor(i / 2M)] / 3) + Math.Floor(GoldCurve[(int)Math.Floor(i / 3M)] / 4)) / 5) * 5, 1e9M);
                GoldCurve.Add(result);

            }
            for (int i = 0; i < 360; i++)
            {
                GoldCurve.Add(1e9M);
            }
        }
    }
}


