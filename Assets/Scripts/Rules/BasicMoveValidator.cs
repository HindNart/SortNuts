public class BasicMoveValidator : IRuleValidator
{
    public bool IsValidMove(Rod from, Rod to)
    {
        if (from == null || to == null || from == to) return false;
        if (from.IsComplete()) return false;
        Nut topNut = from.PeekNut();
        if (topNut == null) return false;

        return to.CanPlaceNut(topNut, false);
    }
}