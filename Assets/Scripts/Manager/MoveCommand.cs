using System.Collections;
using DG.Tweening;
using UnityEngine;

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
        movedNut = toRod.RemoveNut();
        if (movedNut != null)
        {
            NutMove(movedNut);
            fromRod.PlaceNut(movedNut);
        }
    }

    private IEnumerator NutMove(Nut movedNut)
    {
        // Khôi phục vị trí
        // Nhấc nut lên
        movedNut.transform.DOMove(movedNut.transform.position + Vector3.up * 2, 0.3f);
        // Move hợp lệ => Di chuyển ngang và hạ xuống
        Vector3 horizontalTarget = new Vector3(fromRod.transform.position.x, movedNut.transform.position.y, movedNut.transform.position.z);
        yield return movedNut.transform.DOMove(horizontalTarget, 0.4f).SetEase(Ease.InOutQuad).WaitForCompletion();

        Vector3 finalPos = fromRod.GetNutPosition(fromRod.GetNutCount);
        yield return movedNut.transform.DOMove(finalPos, 0.3f).SetEase(Ease.InQuad).WaitForCompletion();
    }
}