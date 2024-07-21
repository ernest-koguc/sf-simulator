namespace SFSimulator.Core;
public class Schedule
{
    public List<ScheduleWeeksDTO> ScheduleWeeks { get; set; } = new();
}

public class ScheduleWeeksDTO
{
    public List<ScheduleDayDTO> ScheduleDays { get; set; } = new();
}
public class ScheduleDayDTO
{
    public List<string> Actions { get; set; } = new();
    public List<string> Events { get; set; } = new();
}