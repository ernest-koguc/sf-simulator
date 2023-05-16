using System.Globalization;

namespace SFSimulator.Core
{
    public class ValuesReader : IValuesReader
    {
        private readonly string _xpForNextLevelPath = Path.Combine(Environment.CurrentDirectory + "/Data", "RequiredExperience.txt");

        private readonly string _itemGoldValuesPath = Path.Combine(Environment.CurrentDirectory + "/Data", "ItemGoldValues.txt");

        public Dictionary<int, int> ReadExperienceForNextLevel()
        {
            Dictionary<int, int> result = new();

            using var reader = new StreamReader(_xpForNextLevelPath);
            string? nextLine;
            var level = 0;
            while ((nextLine = reader.ReadLine()) != null)
            {
                var xpString = nextLine.Trim(',');
                var xp = int.Parse(xpString, CultureInfo.InvariantCulture);
                result.Add(level, xp);
                level++;
            }
            return result;
        }

        public Dictionary<int, float> ReadItemGoldValues()
        {
            Dictionary<int, float> result = new();

            using var reader = new StreamReader(_itemGoldValuesPath);
            string? nextLine;
            var level = 0;

            while ((nextLine = reader.ReadLine()) != null)
            {
                level++;
                var goldValue = float.Parse(nextLine, CultureInfo.InvariantCulture);
                result.Add(level, goldValue);
            }

            return result;
        }
    }
}