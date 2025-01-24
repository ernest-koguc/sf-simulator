using Microsoft.AspNetCore.Components;
using Radzen;

namespace SFSimulator.Frontend.Dialogs;

public partial class EndpointDialog : ComponentBase, IDisposable
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    [Inject]
    private NotificationService NotificationService { get; set; } = default!;
    public static DefDialogOptions PreferredDialogOptions => new()
    {
        Width = "80%",
        Height = "80%",
        ShowClose = false,
        ShowTitle = false,
        ContentCssClass = "endpoint-dialog-wrapper",
        CssClass = "bg-transparent"
    };

    private void OnSfToolsClose(CloseWindowMessage closeWindowMessage) => DialogService.Close();
    private void OnSfToolsEndpoint(EndpointDataMessage endpointDataMessage)
    {
        var data = endpointDataMessage.Data;
        DialogService.Close(data);
        NotificationService.Info("Data polled from SFTools successfully");
    }

    protected override void OnInitialized()
    {
        SfToolsComLayer.CloseWindowEvent += OnSfToolsClose;
        SfToolsComLayer.EndpointDataEvent += OnSfToolsEndpoint;
    }

    public void Dispose()
    {
        SfToolsComLayer.CloseWindowEvent -= OnSfToolsClose;
        SfToolsComLayer.EndpointDataEvent -= OnSfToolsEndpoint;
    }
}
