using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Rod selectedRod;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Rod rod = hit.collider.GetComponent<Rod>();
                if (rod != null)
                {
                    if (selectedRod == null)
                    {
                        selectedRod = rod;
                    }
                    else
                    {
                        GameManager.Instance.TryMove(selectedRod, rod);
                        selectedRod = null;
                    }
                }
            }
        }
    }
}