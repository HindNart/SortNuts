public class MoveCommand : ICommand
{
    private readonly Rod fromRod;
    private readonly Rod toRod;
    private Nut movedNut;

    public MoveCommand(Rod fromRod, Rod toRod)
    {
        this.fromRod = fromRod;
        this.toRod = toRod;
    }

    public void Execute()
    {
        movedNut = fromRod.RemoveNut();
        if (movedNut != null)
        {
            toRod.PlaceNut(movedNut);

            // Sau khi remove, nut mới trên cùng của fromRod (nếu có) cần hiện màu nếu đang bị ẩn
            Nut newTop = fromRod.PeekNut();
            if (newTop != null && newTop.IsColorHidden)
            {
                // Hiện lại màu gốc
                newTop.SetColorHidden(false);
            }
        }
    }

    public void Undo()
    {
        if (movedNut != null)
        {
            toRod.RemoveNut();
            fromRod.PlaceNut(movedNut);
        }
    }
}