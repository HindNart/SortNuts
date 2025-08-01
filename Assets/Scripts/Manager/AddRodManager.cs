using UnityEngine;

public class AddRodManager : MonoBehaviour
{
    [SerializeField] private GameObject rodPrefab;
    [SerializeField] private Transform rodParent;
    [SerializeField] private float rodSpacing = 2f;
    [SerializeField] private int costAddRod = 100;

    public void TryAddRod()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");

        if (GoldManager.Instance.SpendGold(costAddRod))
        {
            AddNewRod();
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    private void AddNewRod()
    {
        int rodCount = GameManager.Instance.CurrentRodCount;
        int rodsPerRow = 3;
        float rowSpacing = 3f;

        int row = rodCount / rodsPerRow;
        int col = rodCount % rodsPerRow;
        float x = col * rodSpacing - ((Mathf.Min(rodCount + 1, rodsPerRow) - 1) * rodSpacing / 2f);
        float z = -row * rowSpacing;
        Vector3 position = new Vector3(x, rodParent.transform.localPosition.y, z);

        GameObject rodObj = Instantiate(rodPrefab, position, Quaternion.identity, rodParent);
        Rod newRod = rodObj.GetComponent<Rod>();
        GameManager.Instance.RegisterNewRod(newRod);
        Debug.Log("New rod added!");
    }
}