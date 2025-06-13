namespace SFSimulator.Core;
public class PetPathProvider : IPetPathProvider
{
    private List<int> ShadowPath = [3, 14];
    private List<int> LightPath = [3, 13, 18, 18];
    private List<int> EarthPath = [3, 14, 18];
    private List<int> FirePath = [3, 7, 16];
    private List<int> WaterPath = [3, 10, 16];

    public Pet? GetPetFromPath(PetElementType elementType, PetsState petsState)
    {
        var path = elementType switch
        {
            PetElementType.Shadow => ShadowPath,
            PetElementType.Light => LightPath,
            PetElementType.Earth => EarthPath,
            PetElementType.Fire => FirePath,
            PetElementType.Water => WaterPath,
            _ => throw new ArgumentOutOfRangeException(nameof(elementType), "Provided pet element type is unsupported")
        };

        var pet = petsState.Elements[elementType]
            .Where(p => path.Contains(p.Position) && p.Level >= 1)
            .OrderByDescending(p => p.Position)
            .FirstOrDefault();

        return pet;
    }
}
