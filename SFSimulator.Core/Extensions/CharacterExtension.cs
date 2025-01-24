﻿namespace SFSimulator.Core
{
    public static class CharacterExtension
    {
        public static void PerformAction(this SimulationContext simulationOptions, ThirstSimulationOptions thirstOptions, ActionType action)
        {
            switch (action)
            {
                case ActionType.BuyGriffin:
                    ChangeMount(simulationOptions, thirstOptions, MountType.Griffin);
                    break;
                case ActionType.BuyTiger:
                    ChangeMount(simulationOptions, thirstOptions, MountType.Tiger);
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
                    UpgradeGoldPit(simulationOptions);
                    break;
                case ActionType.UpgradeTreasury:
                    UpgradeTreasury(simulationOptions);
                    break;
                case ActionType.UpgradeAcademy:
                    UpgradeAcademy(simulationOptions);
                    break;
                case ActionType.AddOneHydraHead:
                    AddOneHydraHead(simulationOptions);
                    break;
                default:
                    throw new NotImplementedException("This action is not supported");
            }
        }

        private static void AddOneHydraHead(SimulationContext simulationOptions)
        {
            if (simulationOptions.HydraHeads < 20)
                simulationOptions.HydraHeads++;
        }

        private static void UpgradeAcademy(SimulationContext simulationOptions)
        {
            if (simulationOptions.AcademyLevel < 20)
                simulationOptions.AcademyLevel++;
        }

        private static void UpgradeTreasury(SimulationContext simulationOptions)
        {
            if (simulationOptions.TreasuryLevel < 45)
                simulationOptions.TreasuryLevel++;
        }
        private static void UpgradeGoldPit(SimulationContext simulationOptions)
        {
            if (simulationOptions.GoldPitLevel < 100)
                simulationOptions.GoldPitLevel++;
        }

        private static void ChangeDailySpins(SimulationContext simulationOptions, SpinAmountType spinAmount)
        {
            simulationOptions.SpinAmount = spinAmount;
        }

        private static void ChangeDailyBeers(SimulationContext simulationOptions, int beers)
        {
            simulationOptions.DailyThirst = 100 + beers * 20;
        }

        private static void ChangeMount(SimulationContext simulationOptions, ThirstSimulationOptions thirstOptions, MountType mount)
        {
            simulationOptions.Mount = mount;
            thirstOptions.Mount = mount;
        }

    }
}