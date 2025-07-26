using UnityEngine;
using DG.Tweening;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    private Rod selectedRod;
    private Nut selectedNut;
    private bool isMoving = false;
    private Vector3 nutOriginalPos;

    [SerializeField] private float liftHeight = 2f;
    [SerializeField] private float liftDuration = 0.25f;
    [SerializeField] private float moveHorizontalDuration = 0.4f;
    [SerializeField] private float lowerDuration = 0.25f;

    private void Update()
    {
        if (isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Rod rod = hit.collider.GetComponent<Rod>();
                if (rod != null)
                {
                    HandleRodClick(rod);
                }
            }
        }
    }

    private void HandleRodClick(Rod rod)
    {
        if (selectedRod == null)
        {
            // Chọn rod đầu tiên và nâng nut trên cùng
            Nut topNut = rod.PeekNut();
            if (topNut == null) return;

            selectedRod = rod;
            selectedNut = topNut;
            nutOriginalPos = selectedNut.transform.position;

            isMoving = true;
            selectedNut.transform.DOMove(nutOriginalPos + Vector3.up * liftHeight, liftDuration)
                .OnComplete(() => isMoving = false);
        }
        else
        {
            if (rod == selectedRod)
            {
                // Click lại rod đã chọn => hạ nut xuống
                StartCoroutine(LowerNutBack());
            }
            else
            {
                // Thử di chuyển nut sang rod đích
                StartCoroutine(TryMoveNut(selectedRod, rod));
            }
        }
    }

    private IEnumerator LowerNutBack()
    {
        isMoving = true;
        yield return selectedNut.transform.DOMove(nutOriginalPos, lowerDuration).WaitForCompletion();
        ClearSelection();
        isMoving = false;
    }

    private IEnumerator TryMoveNut(Rod fromRod, Rod toRod)
    {
        if (selectedNut == null)
        {
            ClearSelection();
            yield break;
        }

        isMoving = true;

        // Kiểm tra move hợp lệ
        if (!toRod.CanPlaceNut(selectedNut, false))
        {
            // Không hợp lệ => hạ xuống vị trí ban đầu
            yield return selectedNut.transform.DOMove(nutOriginalPos, lowerDuration).WaitForCompletion();
            ClearSelection();
            isMoving = false;
            yield break;
        }

        // Move hợp lệ => Di chuyển ngang và hạ xuống
        Vector3 horizontalTarget = new Vector3(toRod.transform.position.x, selectedNut.transform.position.y, selectedNut.transform.position.z);
        yield return selectedNut.transform.DOMove(horizontalTarget, moveHorizontalDuration).SetEase(Ease.InOutQuad).WaitForCompletion();

        Vector3 finalPos = toRod.GetNutPosition(toRod.GetNutCount);
        yield return selectedNut.transform.DOMove(finalPos, lowerDuration).SetEase(Ease.InQuad).WaitForCompletion();

        // Cập nhật logic
        GameManager.Instance.TryMove(fromRod, toRod);

        ClearSelection();
        isMoving = false;
    }

    private void ClearSelection()
    {
        selectedRod = null;
        selectedNut = null;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}