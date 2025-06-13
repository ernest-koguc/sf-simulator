using Microsoft.AspNetCore.Components;
using Radzen;

namespace SFSimulator.Frontend.Dialogs;

public partial class CrashReportDialog
{
    [Parameter, EditorRequired]
    public required string Message { get; set; }
    [Parameter, EditorRequired]
    public required string Details { get; set; }
    public bool ShowDetails { get; set; } = false;
    public static DialogOptions DialogOptions { get; } = new()
    {
        Width = "600px",
        Height = "400px",
        CloseDialogOnEsc = true,
        CloseDialogOnOverlayClick = true,
        AutoFocusFirstElement = false,
        ContentCssClass = "h-100"
    };
    public static string Title { get; } = "Seems like something went wrong";

    [Inject]
    private DialogService DialogService { get; set; } = default!;
}
