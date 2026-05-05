using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    private GameObject buildingPrefab;
    private GameObject previewInstance;
    public Material previewMaterial;

    private int placementMask;

    public bool IsPlacingBuilding => buildingPrefab != null;

    void Awake()
    {
        Instance = this;
        placementMask = LayerMask.GetMask("Mineral", "Unit", "Building");
    }

    public void StartPlacingBuilding(GameObject prefab)
    {
        buildingPrefab = prefab;
        if (previewInstance != null)
            Destroy(previewInstance);

        previewInstance = Instantiate(prefab);

        foreach (var col in previewInstance.GetComponentsInChildren<Collider>())
            col.enabled = false;
        foreach (var obs in previewInstance.GetComponentsInChildren<NavMeshObstacle>())
            obs.enabled = false;

        SetTransparent(previewInstance, previewMaterial);
    }

    void Update()
    {
        if (!IsPlacingBuilding || previewInstance == null)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPlacement();
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int groundMask = LayerMask.GetMask("Ground");
        if (!Physics.Raycast(ray, out var hit, 100f, groundMask))
            return;

        Vector3 placePos = hit.point;
        placePos.x = Mathf.Round(placePos.x);
        placePos.z = Mathf.Round(placePos.z);
        var rend = previewInstance.GetComponentInChildren<Renderer>();
        placePos.y = hit.point.y + rend.bounds.extents.y;
        previewInstance.transform.position = placePos;

        if (Input.GetMouseButtonDown(0))
        {
            Bounds bounds = rend.bounds;
            var hits = Physics.OverlapBox(
                bounds.center,
                bounds.extents,
                previewInstance.transform.rotation,
                placementMask
            );
            if (hits.Length > 0)
            {
                Debug.LogWarning("오브젝트 겹침, 건설 불가");
                return;
            }

            var unitGO = UnitSelectionManager.Instance.GetFirstSelectedUnit();
            if (unitGO != null)
            {
                var builder = unitGO.GetComponent<IBuilder>();
                if (builder != null)
                {
                    Transform spawn = previewInstance.transform.Find("SCVSpawnPoint");
                    Vector3 spawnPos = (spawn != null) ? spawn.position : placePos;

                    builder.OrderBuild(buildingPrefab, placePos, spawnPos);
                    CancelPlacement();
                }
                else
                {
                    Debug.LogWarning("선택된 유닛이 건설 가능 유닛이 아님");
                }
            }
            else
            {
                Debug.LogWarning("유닛이 선택되지 않음");
            }
        }
    }

    void CancelPlacement()
    {
        buildingPrefab = null;
        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
    }

    void SetTransparent(GameObject obj, Material mat)
    {
        foreach (var r in obj.GetComponentsInChildren<Renderer>())
            r.material = mat;
    }
}