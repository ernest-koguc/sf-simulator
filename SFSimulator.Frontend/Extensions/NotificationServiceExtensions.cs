using Radzen;

namespace SFSimulator.Frontend;

public static class NotificationServiceExtensions
{
    public static void Info(this NotificationService service, string summary, string detail = "")
    {
        Console.WriteLine("INFO: " + summary + detail);
        service.Notify(NotificationSeverity.Info, summary, detail);
    }
    public static void Success(this NotificationService service, string summary, string detail = "")
    {
        Console.WriteLine("SUCCESS: " + summary + detail);
        service.Notify(NotificationSeverity.Success, summary, detail);
    }
    public static void Warning(this NotificationService service, string summary, string detail = "")
    {
        Console.WriteLine("WARNING: " + summary + detail);
        service.Notify(NotificationSeverity.Warning, summary, detail);
    }
    public static void Error(this NotificationService service, string summary, string detail = "")
    {
        Console.WriteLine("ERROR: " + summary + detail);
        service.Notify(NotificationSeverity.Error, summary, detail, 6000);
    }
}
