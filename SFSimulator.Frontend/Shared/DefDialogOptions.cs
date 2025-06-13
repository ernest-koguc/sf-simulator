using Radzen;

namespace SFSimulator.Frontend;

public class DefDialogOptions : DialogOptions
{
    public DefDialogOptions()
    {
        CloseDialogOnEsc = true;
        CloseDialogOnOverlayClick = true;
    }
}
