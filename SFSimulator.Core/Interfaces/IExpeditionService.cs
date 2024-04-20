namespace SFSimulator.Core;

public interface IExpeditionService
{
    public ExpeditionOptions Options { get; set; }
    public decimal GetDailyExpeditionGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, int thirst);
    public long GetDailyExpeditionExperience(int characterLevel, ExperienceBonus experienceBonus, bool isExperienceEvent, MountType mount, int thirst);
}

