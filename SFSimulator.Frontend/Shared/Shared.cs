using Radzen;

namespace SFSimulator.Frontend;

public static class Shared
{
    public static DialogOptions DefaultDialogOptions => new() { CloseDialogOnEsc = true, CloseDialogOnOverlayClick = true };
}
