using SFSimulator.Core;

namespace SFSimulator.Frontend;

//[MagicTable("SimulationResult", Constants.DatabaseName)]
public class SavedResultEntity
{
    //[MagicPrimaryKey("Id")]
    public int Id { get; set; }
    //[MagicIndex("Saved")]
    public required DateTime Saved { get; set; }
    //[MagicIndex("Result")]
    public required SimulationResult Result { get; set; }
}

