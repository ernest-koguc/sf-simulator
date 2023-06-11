namespace SFSimulator.Core
{
    public static class CharacterExtension
    {
        public static void PerformAction(this Character character, SimulationOptions simulationOptions, ThirstSimulationOptions thirstOptions, ActionType action) 
        {
            switch (action)
            {
                case ActionType.BuyGriffin:
                    ChangeMount(character, thirstOptions, MountType.Griffin);
                    break;
                case ActionType.BuyTiger:
                    ChangeMount(character, thirstOptions, MountType.Tiger); 
                    break;
                case ActionType.SpinMaxTimes:
                    ChangeDailySpins(simulationOptions, SpinAmountType.Max);
                    break;  
                case ActionType.SpinOnce:
                    ChangeDailySpins(simulationOptions, SpinAmountType.OnlyFree); 
                    break;
                case ActionType.DrinkAllBeers:
                    ChangeDailyBeers(simulationOptions, 11); 
                    break;
                case ActionType.DrinkOneBeer:
                    ChangeDailyBeers(simulationOptions, 1);
                    break;
                case ActionType.UpgradeGoldPit:
                    UpgradeGoldPit(character);
                    break;
                case ActionType.UpgradeTreasury:
                    UpgradeTreasury(character);
                    break;
                case ActionType.UpgradeAcademy:
                    UpgradeAcademy(character);
                    break;
                default:
                    throw new NotImplementedException("This action is not supported");
            }
        }

        private static void UpgradeAcademy(Character character)
        {
            if (character.AcademyLevel < 20)
                character.AcademyLevel++;
        }

        private static void UpgradeTreasury(Character character)
        {
            if (character.TreasuryLevel < 45)
                character.TreasuryLevel++;
        }
        private static void UpgradeGoldPit(Character character)
        {
            if (character.GoldPitLevel < 100)
                character.GoldPitLevel++;
        }

        private static void ChangeDailySpins(SimulationOptions simulationOptions, SpinAmountType spinAmount)
        {
            simulationOptions.SpinAmount = spinAmount;
        }

        private static void ChangeDailyBeers(SimulationOptions simulationOptions, int beers)
        {
            simulationOptions.DailyThirst = 100 + beers * 20;
        }

        private static void ChangeMount(Character character, ThirstSimulationOptions thirstOptions, MountType mount)
        {
            character.Mount = mount;
            thirstOptions.Mount = mount;
        }

    }
}
