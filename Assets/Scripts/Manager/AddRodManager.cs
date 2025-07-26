using UnityEngine;

public class AddRodManager : MonoBehaviour
{
    [SerializeField] private GameObject rodPrefab;
    [SerializeField] private Transform rodParent;
    [SerializeField] private float rodSpacing = 2f;
    [SerializeField] private int costAddRod = 100;

    public void TryAddRod()
    {
        if (UIManager.Instance.SpendGold(costAddRod))
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
        Vector3 position = new Vector3(rodCount * rodSpacing - ((rodCount) * rodSpacing / 2f), -3f, 0);
        GameObject rodObj = Instantiate(rodPrefab, position, Quaternion.identity, rodParent);

        Rod newRod = rodObj.GetComponent<Rod>();
        GameManager.Instance.RegisterNewRod(newRod);
        Debug.Log("New rod added!");
    }
}