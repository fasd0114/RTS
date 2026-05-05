using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    [Header("SCV 생산 설정")]
    public GameObject scvPrefab;
    public Transform spawnPoint;
    public int scvCost = 50;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterCommandCenter(this);
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterCommandCenter(this);
        }
    }
    void OnMouseDown()
    {
        UnitProductionUI.Instance.ShowCommandCenter(this);
    }

    public void ProduceSCV()
    {
        if (GameManager.Instance.SpendResources(scvCost))
        {
            Instantiate(scvPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
