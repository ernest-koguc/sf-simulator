using Microsoft.JSInterop;

namespace SFSimulator.Frontend;

public class BrowserService(IJSRuntime jsRuntime)
{
    public async Task<WindowDimension> GetWindowDimensionAsync() => await jsRuntime.InvokeAsync<WindowDimension>("getDimensions");
    public static event Func<Task>? OnResize;
    [JSInvokable]
    public static Task WindowsResize()
    {
        if (OnResize is null) return Task.CompletedTask;
        return OnResize.Invoke();
    }
}

public readonly record struct WindowDimension(int Width, int Height);
