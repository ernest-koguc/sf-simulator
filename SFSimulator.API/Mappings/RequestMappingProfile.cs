using AutoMapper;
using SFSimulator.API.Requests;
using SFSimulator.Core;
using static SFSimulator.API.Mappings.StaticMappingFunctions;

namespace SFSimulator.API.Mappings;

public class RequestMappingProfile : Profile
{
    public RequestMappingProfile()
    {
        _ = CreateMap<SimulateRequest, SimulationOptions>()
            .ForMember(d => d.SwitchPriority, m => m.MapFrom(s => s.SwitchPriority))
            .ForMember(d => d.SwitchLevel, m => m.MapFrom(s => s.SwitchLevel))
            .ForMember(d => d.FightsForGold, m => m.MapFrom(s => s.FightsForGold))
            .ForMember(d => d.GoldBonus, m => m.MapFrom((s, _) =>
            {
                return new GoldBonus(s.Tower, s.GoldGuildBonus, s.GoldRuneBonus, s.HasGoldScroll, s.HasArenaGoldScroll);
            }))
            .ForMember(d => d.ExperienceBonus, m => m.MapFrom((s, _) =>
            {
                return new ExperienceBonus(s.ScrapbookFillness, s.XpGuildBonus, s.XpRuneBonus, s.HasExperienceScroll);
            }))
            .ForMember(d => d.DrinkBeerOneByOne, m => m.MapFrom(s => s.DrinkBeerOneByOne))
            .ForMember(d => d.DailyThirst, m => m.MapFrom(s => s.DailyThirst))
            .ForMember(d => d.Schedule, m => m.MapFrom(s => MapSchedule(s.Schedule)))
            .ForMember(d => d.WeeklyTasksOptions, m => m.MapFrom(s => new WeeklyTasksOptions(s.DoWeeklyTasks, s.DrinkExtraWeeklyBeer)))
            .ForMember(d => d.ExpeditionOptions, m => m.MapFrom(s => s.ExpeditionOptions))
            .ForMember(d => d.ExpeditionOptionsAfterSwitch, m => m.MapFrom(s => s.ExpeditionOptionsAfterSwitch))
            .ForMember(d => d.QuestOptions, m => m.MapFrom(s => s.QuestOptions))
            .ForMember(d => d.QuestOptionsAfterSwitch, m => m.MapFrom(s => s.QuestOptionsAfterSwitch))
            .ForMember(d => d.ExpeditionsInsteadOfQuests, m => m.MapFrom(s => s.ExpeditionsInsteadOfQuests))
            .ForMember(d => d.Mount, m => m.MapFrom(s => s.MountType))
            //.ForMember(d => d.GuildKnights, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.GuildKnights : 0))
            //.ForMember(d => d.BlackSmithResources, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BlackSmithResources : default))
            //.ForMember(d => d.Potions, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.Potions : null))
            //.ForMember(d => d.Class, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.Class : ClassType.Bert))
            //.ForMember(d => d.GladiatorLevel, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.GladiatorLevel : 0))
            //.ForMember(d => d.SoloPortal, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.SoloPortal / 100D : 0D))
            //.ForMember(d => d.GuildPortal, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.GuildPortal / 100D : 0D))
            //.ForMember(d => d.BaseStrength, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BaseStrength : 0))
            //.ForMember(d => d.BaseDexterity, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BaseDexterity : 0))
            //.ForMember(d => d.BaseIntelligence, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BaseIntelligence : 0))
            //.ForMember(d => d.BaseConstitution, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BaseConstitution : 0))
            //.ForMember(d => d.BaseLuck, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BaseLuck : 0))
            //.ForMember(d => d.Aura, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.Aura : 0))
            //.ForMember(d => d.BaseStrength, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BaseStrength : 0))
            //.ForMember(d => d.BaseStrength, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BaseStrength : 0))
            //.ForMember(d => d.BaseStrength, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.BaseStrength : 0))
            //.ForMember(d => d.Pets, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.Pets : null))
            //.ForMember(d => d.Companions, m => m.MapFrom(s => s.DungeonOptions != null ? s.DungeonOptions.Companions : null))
            //.ForMember(d => d.Items, m => m.MapFrom(s => s.DungeonOptions != null ? new FightableItems(s.DungeonOptions.Class, s.DungeonOptions.Items) : null))
            ;


        _ = CreateMap<Maria21DataDTO, ExperienceBonus>()
            .ForMember(d => d.ScrapbookFillness, m => m.MapFrom(s => Math.Round(s.Book / (decimal)CoreShared.SCRAPBOOK_LIMIT * 100, 3)))
            .ForMember(d => d.GuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.XP)))
            .ForMember(d => d.RuneBonus, m => m.MapFrom(s => GetQuestRuneBonus(s, BonusType.XP)))
            .ForMember(d => d.HasExperienceScroll, m => m.MapFrom(s => s.Items.Head.Enchantment == WitchScrollType.QuestExperience || s.Inventory.Dummy.Head.Enchantment == WitchScrollType.QuestExperience))
            ;

        _ = CreateMap<Maria21DataDTO, GoldBonus>()
            .ForMember(d => d.Tower, m => m.MapFrom(s => Math.Max(s.Dungeons.Tower, 0)))
            .ForMember(d => d.GuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.GOLD)))
            .ForMember(d => d.RuneBonus, m => m.MapFrom(s => GetQuestRuneBonus(s, BonusType.GOLD)))
            .ForMember(d => d.HasGoldScroll, m => m.MapFrom(s => s.Items.Ring.Enchantment == WitchScrollType.QuestGold || s.Inventory.Dummy.Ring.Enchantment == WitchScrollType.QuestGold))
            .ForMember(d => d.HasArenaGoldScroll, m => m.MapFrom(s => s.Items.Misc.Enchantment == WitchScrollType.ArenaGold || s.Inventory.Dummy.Misc.Enchantment == WitchScrollType.ArenaGold))
            ;

        _ = CreateMap<Maria21DataDTO, SimulationOptions>()
            .ForMember(d => d.ExperienceBonus, m => m.MapFrom(s => s))
            .ForMember(d => d.GoldBonus, m => m.MapFrom(s => s))
            .ForMember(d => d.Level, m => m.MapFrom(s => s.Level))
            .ForMember(d => d.Experience, m => m.MapFrom(s => s.XP))
            .ForMember(d => d.GoldPitLevel, m => m.MapFrom(s => s.Underworld.GoldPit))
            .ForMember(d => d.AcademyLevel, m => m.MapFrom(s => s.Fortress.Academy))
            .ForMember(d => d.GemMineLevel, m => m.MapFrom(s => s.Fortress.GemMine))
            .ForMember(d => d.TreasuryLevel, m => m.MapFrom(s => s.Fortress.Treasury))
            .ForMember(d => d.Mount, m => m.MapFrom(s => s.Mount))
            .ForMember(d => d.BaseStat, m => m.MapFrom(s => SumBaseStats(s)))
            .ForMember(d => d.HydraHeads, m => m.MapFrom(s => s.Group.Group.Hydra))
            .ForMember(d => d.Class, m => m.MapFrom(s => s.Class))
            .ForMember(d => d.BaseStrength, m => m.MapFrom(s => s.Strength.Base))
            .ForMember(d => d.BaseDexterity, m => m.MapFrom(s => s.Dexterity.Base))
            .ForMember(d => d.BaseIntelligence, m => m.MapFrom(s => s.Intelligence.Base))
            .ForMember(d => d.BaseConstitution, m => m.MapFrom(s => s.Constitution.Base))
            .ForMember(d => d.BaseLuck, m => m.MapFrom(s => s.Luck.Base))
            .ForMember(d => d.GladiatorLevel, m => m.MapFrom(s => s.Fortress.Gladiator))
            .ForMember(d => d.SoloPortal, m => m.MapFrom(s => s.Dungeons.Player))
            .ForMember(d => d.GuildPortal, m => m.MapFrom(s => s.Dungeons.Group))
            .ForMember(d => d.Calendar, m => m.MapFrom(s => s.CalendarType))
            .ForMember(d => d.CalendarDay, m => m.MapFrom(s => s.CalendarDay == 0 ? 1 : s.CalendarDay))
            .ForMember(d => d.Items, m => m.MapFrom(s => new FightableItems(s.Class, MapItems(s.Class, s.Items))))
            .ForMember(d => d.DungeonsData, m => m.MapFrom(s => s.Dungeons))
            .ForMember(d => d.Potions, m => m.MapFrom(s => s.Potions))
            .ForMember(d => d.Aura, m => m.MapFrom(s => s.Toilet.Aura))
            .ForMember(d => d.GuildKnights, m => m.MapFrom(s => s.Fortress.Knights))
            .ForMember(d => d.Companions, m => m.MapFrom(s => new List<SFToolsCompanion>() { s.Companions.Bert, s.Companions.Mark, s.Companions.Kunigunde }))
            .ForMember(d => d.Pets, m => m.MapFrom(s => new PetsState(s.Pets)))
            ;

        _ = CreateMap<SFToolsCompanion, Companion>()
            .ForMember(d => d.Class, m => m.MapFrom(s => s.Class))
            .ForMember(d => d.Items, m => m.MapFrom(s => new FightableItems(s.Class, MapItems(s.Class, s.Items))))
            ;

        _ = CreateMap<SFToolsItem, RawWeapon>()
            .ForMember(d => d.MinDmg, m => m.MapFrom(s => s.DamageMin))
            .ForMember(d => d.MaxDmg, m => m.MapFrom(s => s.DamageMax))
            ;
    }
}