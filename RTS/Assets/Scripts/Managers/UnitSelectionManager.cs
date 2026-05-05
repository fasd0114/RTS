using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance;

    // 다중 선택 유닛 리스트
    private List<GameObject> selectedUnits = new List<GameObject>();

    // 각 유닛별 인디케이터 인스턴스 관리 딕셔너리
    private Dictionary<GameObject, GameObject> selectionIndicators = new Dictionary<GameObject, GameObject>();

    [Header("Selection Indicator")]
    [Tooltip("원형 평면(Quad 등)에 원형 텍스처가 적용된 프리팹")]
    public GameObject selectionIndicatorPrefab;

    void Awake()
    {
        Instance = this; 
    }

    void Update()
    {
        // ESC 키: 모든 선택 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearSelection();
            return;
        }

        // 좌클릭: 단일 및 컨트롤 다중 선택
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (BuildManager.Instance != null && BuildManager.Instance.IsPlacingBuilding) return;

            HandleSelection();
        }

        // 우클릭: 선택된 모든 유닛에게 명령 하달
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            HandleOrders();
        }
    }

    void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            var fac = hit.collider.GetComponent<FactionComponent>();

            if (fac != null && fac.faction == Faction.Player)
            {
                GameObject clickedObject = hit.collider.gameObject;

                // 컨트롤 키를 누르지 않았다면 기존 선택 모두 해제
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    ClearSelection();
                }

                // 이미 선택된 유닛이면 제거, 아니면 추가
                if (selectedUnits.Contains(clickedObject))
                {
                    RemoveFromSelection(clickedObject);
                }
                else
                {
                    AddToSelection(clickedObject);
                }

                ApplyUIState();
            }
            else
            {
                // 빈 땅이나 적 클릭 시 (컨트롤 키 안 누른 상태면 전체 해제)
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    ClearSelection();
                }
            }
        }
    }

    void AddToSelection(GameObject unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            CreateIndicator(unit);
        }
    }

    void RemoveFromSelection(GameObject unit)
    {
        if (selectedUnits.Contains(unit))
        {
            selectedUnits.Remove(unit);
            DestroyIndicator(unit);
        }
    }

    public void ClearSelection()
    {
        // 모든 인디케이터 삭제
        foreach (var indicator in selectionIndicators.Values)
        {
            Destroy(indicator);
        }
        selectionIndicators.Clear();
        selectedUnits.Clear();

        // 모든 UI 닫기
        BuildUI.Instance.Hide();
        UnitProductionUI.Instance.HideAll();
    }

    void ApplyUIState()
    {
        BuildUI.Instance.Hide();
        UnitProductionUI.Instance.HideAll();

        // 유닛이 하나만 선택되었을 때만 패널을 열어줌
        if (selectedUnits.Count == 1)
        {
            GameObject unit = selectedUnits[0];
            if (unit.CompareTag("SCV"))
            {
                BuildUI.Instance.Show();
            }
            else if (unit.CompareTag("Building"))
            {
                // 건물 확인
                if (unit.TryGetComponent<CommandCenter>(out var cc))
                {
                    UnitProductionUI.Instance.ShowCommandCenter(cc);
                }
                else if (unit.TryGetComponent<Barracks>(out var barracks))
                {
                    UnitProductionUI.Instance.ShowBarracks(barracks);
                }
            }
        }
    }

    void HandleOrders()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit)) return;

        var targetFac = hit.collider.GetComponent<FactionComponent>();

        foreach (var unit in selectedUnits)
        {
            if (unit.TryGetComponent<SCV>(out var scv))
            {
                if (hit.collider.CompareTag("Mineral"))
                    scv.OrderMine(hit.collider.gameObject);
                else
                    scv.OrderMove(hit.point);
            }
            else if (unit.TryGetComponent<Marine>(out var marine))
            {
                if (targetFac != null && targetFac.faction == Faction.Enemy)
                {
                    // 적이라면 공격 명령
                    marine.Attack(hit.collider.gameObject);
                }
                else
                {
                    // 빈 땅이거나 아군이라면 이동 명령
                    marine.MoveTo(hit.point)
                };
            }
        }
    }

    void CreateIndicator(GameObject unit)
    {
        if (selectionIndicators.ContainsKey(unit)) return;

        float indicatorThickness = 0.02f;
        float yOffset = indicatorThickness;
        Collider col = unit.GetComponent<Collider>();

        if (col != null)
        {
            float localHeight = col.bounds.size.y / unit.transform.lossyScale.y;
            yOffset -= (localHeight * 0.5f);
        }

        GameObject indicator;
        if (selectionIndicatorPrefab != null)
        {
            indicator = Instantiate(selectionIndicatorPrefab, unit.transform);
            indicator.transform.localPosition = new Vector3(0f, yOffset, 0f);
            indicator.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            indicator.transform.localScale = Vector3.one * 1.5f;
        }
        else
        {
            // 프리팹이 없을 경우 기본 실린더로 생성
            indicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Destroy(indicator.GetComponent<Collider>());
            indicator.transform.SetParent(unit.transform, false);
            indicator.transform.localPosition = new Vector3(0f, yOffset, 0f);
            indicator.transform.localScale = new Vector3(1.5f, indicatorThickness, 1.5f);

            var mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(0f, 1f, 0f, 0.5f); // 녹색 반투명
            indicator.GetComponent<Renderer>().material = mat;
        }

        selectionIndicators.Add(unit, indicator);
    }

    void DestroyIndicator(GameObject unit)
    {
        if (selectionIndicators.ContainsKey(unit))
        {
            Destroy(selectionIndicators[unit]);
            selectionIndicators.Remove(unit);
        }
    }
    public GameObject GetFirstSelectedUnit()
    {
        return selectedUnits.Count > 0 ? selectedUnits[0] : null;
    }
}
