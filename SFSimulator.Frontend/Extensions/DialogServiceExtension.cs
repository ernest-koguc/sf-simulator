using Radzen;
using SFSimulator.Frontend.Dialogs;

namespace SFSimulator.Frontend.Extensions;

public static class DialogServiceExtension
{
    public static Task OpenProgressBar(this DialogService dialogService, string text)
    {
        var opt = new DialogOptions
        {
            ShowClose = false,
            CloseDialogOnEsc = false,
            CloseDialogOnOverlayClick = false,
            ShowTitle = false

        };
        return dialogService.OpenAsync<ProgressBar>(text);
    }

    public static Task OpenCrashReport(this DialogService dialogService, string message, string details)
    {
        return dialogService.OpenAsync<CrashReportDialog>(CrashReportDialog.Title, new()
            {
                {nameof(CrashReportDialog.Message), message },
                {nameof(CrashReportDialog.Details), details },
            },
            CrashReportDialog.DialogOptions);
    }
}
