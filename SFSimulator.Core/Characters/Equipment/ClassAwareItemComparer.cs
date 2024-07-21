namespace SFSimulator.Core;

public class ClassAwareItemComparer : IComparer<EquipmentItem>
{
    private ClassType ClassType { get; }

    public ClassAwareItemComparer(ClassType classType)
    {
        ClassType = classType;
    }

    public int Compare(EquipmentItem? x, EquipmentItem? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        var mainAttribute = ClassConfigurationProvider.GetClassConfiguration(ClassType).MainAttribute;
        var mainAttributeX = x[mainAttribute];
        var mainAttributeY = y[mainAttribute];

        var totalX = x.Constitution + mainAttributeX;
        var totalY = y.Constitution + mainAttributeY;

        if (totalX > totalY) return 1;
        if (totalX == totalY) return 0;
        return -1;
    }
}