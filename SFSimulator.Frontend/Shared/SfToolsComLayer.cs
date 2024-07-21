using Microsoft.JSInterop;
using SFSimulator.Core;
using System.Text.Json;

namespace SFSimulator.Frontend;

public static class SfToolsComLayer
{
    public delegate void CloseWindowEventHandler(CloseWindowMessage message);
    public static event CloseWindowEventHandler? CloseWindowEvent;

    public delegate void EndpointDataEventHandler(EndpointDataMessage message);
    public static event EndpointDataEventHandler? EndpointDataEvent;

    [JSInvokable]
    public static void PostSfToolsMessage(SfToolsMessage message)
    {
        switch (message.Event)
        {
            case "sftools-close":
                CloseWindowEvent?.Invoke(new());
                break;
            case "sftools-data":
                EndpointDataEvent?.Invoke(new(message.Data!));
                break;
            default:
                throw new NotImplementedException("Unsupported sftools event, object dump: \n" + JsonSerializer.Serialize(message));
        };
    }
}

public class SfToolsMessage
{
    public string? Event { get; set; }
    public Maria21DataDTO? Data { get; set; }
}
public record CloseWindowMessage();
public record EndpointDataMessage(Maria21DataDTO Data);

