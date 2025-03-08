namespace SFSimulator.Core;

public interface IExpeditionService
{
    ExpeditionOptions Options { get; set; }
    decimal GetDailyExpeditionGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, int thirst);
    long GetDailyExpeditionExperience(int characterLevel, ExperienceBonus experienceBonus, bool isExperienceEvent, MountType mount, int thirst);
    List<Item> GetDailyExpeditionItems(int characterLevel, int thirst);
    double GetDailyExpeditionPetFood(int characterLevel, GoldBonus goldBonus, List<EventType> events, MountType mount, int thirst);
}