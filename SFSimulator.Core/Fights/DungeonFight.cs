namespace SFSimulator.Core
{
    public class DungeonFight
    {
        private IFightableContext Character { get; init; }
        private List<IFightableContext>? Companions { get; init; }
        private IFightableContext DungeonEnemy { get; init; }
        private StartingBehaviorEnum StartingBehavior { get; init; }
        private readonly Random _random;
        public DungeonFight(IFightableContext character, IFightableContext dungeonEnemy, StartingBehaviorEnum startingBehavior, Random random, List<IFightableContext>? companions = null)
        {
            _random = random;
            StartingBehavior = startingBehavior;
            Character = character;
            Companions = companions;
            DungeonEnemy = dungeonEnemy;
        }

        public void OnFightEnd()
        {
            Character.ResetState();
            DungeonEnemy.ResetState();
        }

        public bool PerformFight()
        {
            var characterStarts = GetCharacterStarts();

            IFightableContext attacker;
            IFightableContext defender;
            if (characterStarts)
            {
                attacker = Character;
                defender = DungeonEnemy;
            }
            else
            {
                attacker = DungeonEnemy;
                defender = Character;
            }
            var round = 0;

            if (attacker is IBeforeFightAttackable attackerImpl && attackerImpl.AttackBeforeFight(defender, ref round))
            {
                OnFightEnd();
                return characterStarts;
            }

            if (defender is IBeforeFightAttackable defenderImpl && defenderImpl.AttackBeforeFight(defender, ref round))
            {
                OnFightEnd();
                return !characterStarts;
            }

            bool? result = null;

            for (var i = 0; i < int.MaxValue; i++)
            {
                if (attacker.Attack(defender, ref round))
                {
                    result = characterStarts;
                    break;
                }

                if (defender.Attack(attacker, ref round))
                {
                    result = !characterStarts;
                    break;
                }
            }
            if (result is null)
            {
                throw new Exception("Dungeon simulation exceeded max iterations");
            }

            OnFightEnd();
            return result.Value;
        }

        private bool GetCharacterStarts()
        {
            if (StartingBehavior == StartingBehaviorEnum.Random)
            {
                return _random.NextDouble() < 0.5;
            }

            return StartingBehavior == StartingBehaviorEnum.Character;
        }
    }
}