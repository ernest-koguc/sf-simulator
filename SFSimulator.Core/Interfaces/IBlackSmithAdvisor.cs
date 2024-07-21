namespace SFSimulator.Core;

public interface IBlackSmithAdvisor
{
    BlackSmithResources DismantleItem(EquipmentItem item);
    BlackSmithResources UpgradeItems(List<EquipmentItem> items, BlackSmithResources resources);
}