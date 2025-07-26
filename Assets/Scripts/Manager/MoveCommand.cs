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