using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

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

    private float lastClickTime = 0f;
    private float clickCooldown = 0.15f;

    private void Update()
    {
        if (isMoving) return;

        // --- PC: click chuột trái ---
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < clickCooldown) return;
            lastClickTime = Time.time;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return; // Click trúng UI thì bỏ qua

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ProcessRaycast(ray);
        }

        // --- Mobile: chạm màn hình ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (Time.time - lastClickTime < clickCooldown) return;
                lastClickTime = Time.time;

                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return; // Chạm vào UI thì bỏ qua

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                ProcessRaycast(ray);
            }
        }
    }

    private void ProcessRaycast(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Rod rod = hit.collider.GetComponent<Rod>();
            if (rod != null)
            {
                HandleRodClick(rod);
            }
        }
    }

    private void HandleRodClick(Rod rod)
    {
        if (isMoving) return;

        if (selectedRod == null)
        {
            // Chọn rod đầu tiên và nâng nut trên cùng
            Nut topNut = rod.PeekNut();
            if (topNut == null || rod.IsComplete()) return;

            selectedRod = rod;
            selectedNut = topNut;
            nutOriginalPos = selectedNut.transform.position;

            AudioManager.Instance.PlaySFX("LiftNut");


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
        AudioManager.Instance.PlaySFX("DownNut");

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

        AudioManager.Instance.PlaySFX("DownNut");

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