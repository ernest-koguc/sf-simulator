using Radzen;

namespace SFSimulator.Frontend;

public static class SharedUtilitity
{
    public static DialogOptions DefaultDialogOptions => new() { CloseDialogOnEsc = true, CloseDialogOnOverlayClick = true };
}
