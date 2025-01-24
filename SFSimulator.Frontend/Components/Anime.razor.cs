using Microsoft.AspNetCore.Components;

namespace SFSimulator.Frontend.Components;


public partial class Anime
{
    [Parameter]
    public bool ShowInitially { get; set; }
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    private bool ShouldShow { get; set; }
    private bool StartAnimation { get; set; }
    private bool Initalized { get; set; }
    public void Toggle()
    {
        StartAnimation = true;
    }

    private void OnAnimationEnd()
    {
        ShouldShow = !ShouldShow;
        StartAnimation = false;
        StateHasChanged();
    }
    protected override Task OnParametersSetAsync()
    {
        if (!Initalized)
        {
            ShouldShow = ShowInitially;
            Initalized = true;
        }
        return base.OnParametersSetAsync();
    }
}
