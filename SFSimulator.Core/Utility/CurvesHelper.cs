namespace SFSimulator.Core
{
    public class CurvesHelper : ICurvesHelper
    {

        public List<double> GoldCurve { get; } = new(650) { 0, 25, 50, 75 };
        public CurvesHelper()
        {
            for (int i = GoldCurve.Count; i < 640; i++)
            {
                var result = Math.Min(Math.Floor((GoldCurve[i - 1] + Math.Floor(GoldCurve[(int)Math.Floor(i / 2d)] / 3) + Math.Floor(GoldCurve[(int)Math.Floor(i / 3d)] / 4)) / 5) * 5, 1e9);
                GoldCurve.Add(result);

            }
            for (int i = 0; i < 2000; i++)
            {
                GoldCurve.Add(1e9);
            }
        }
    }
}


