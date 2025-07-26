public class BasicMoveValidator : IRuleValidator
{
    public bool IsValidMove(Rod from, Rod to)
    {
        if (from == null || to == null || from == to) return false;
        Nut topNut = from.RemoveNut();
        if (topNut == null) return false;

        bool valid = to.CanPlaceNut(topNut);
        from.PlaceNut(topNut); // Restore
        return valid;
    }
}