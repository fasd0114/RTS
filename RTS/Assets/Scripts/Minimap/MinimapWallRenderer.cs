using UnityEngine;
using UnityEngine.UI;

public class MiniMapWallRenderer : MonoBehaviour
{
    [Header("미니맵 UI")]
    public RectTransform minimapContainer;    // 미니맵을 렌더링하는 UI 패널(Anchored)
    public Image wallIconPrefab;              // 검은색 박스 모양의 Image Prefab

    [Header("맵 크기 (월드 단위)")]
    public float mapWidth = 200f;            // X 방향 전체 길이
    public float mapHeight = 200f;            // Z 방향 전체 길이

    void RefreshWalls()
    {
        // 1) 기존에 그려둔 wall 아이콘 전부 삭제
        foreach (Transform child in minimapContainer)
            Destroy(child.gameObject);

        // 2) 월드상의 Wall 태그 오브젝트들 찾기
        var walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            // 월드 좌표 → 미니맵 로컬 좌표로 변환
            Vector3 wp = wall.transform.position;
            float normalizedX = (wp.x / mapWidth) + 0.5f;  // 0~1 사이
            float normalizedY = (wp.z / mapHeight) + 0.5f;  // 0~1 사이

            // UI 상 위치 (pivot 이 중앙(0.5,0.5) 이라는 가정)
            float uiX = (normalizedX - 0.5f) * minimapContainer.rect.width;
            float uiY = (normalizedY - 0.5f) * minimapContainer.rect.height;

            // 3) Wall 크기(월드 100×40)를 비율로 변환
            Vector3 worldScale = wall.transform.localScale;
            // 만약 콜라이더나 실제 치수로 정확히 잡아야 하면 Collider.bounds.size 사용
            float wallWorldW = 100f;  // 예: 100
            float wallWorldH = 40f;   // 예: 40
            float uiW = (wallWorldW / mapWidth) * minimapContainer.rect.width;
            float uiH = (wallWorldH / mapHeight) * minimapContainer.rect.height;

            // 4) UI Image 생성 및 세팅
            var icon = Instantiate(wallIconPrefab, minimapContainer);
            var rt = icon.rectTransform;
            rt.anchoredPosition = new Vector2(uiX, uiY);
            rt.sizeDelta = new Vector2(uiW, uiH);
            icon.color = Color.black;
        }
    }

    // 필요에 따라 매 프레임, 혹은 일정 주기로 호출
    void LateUpdate()
    {
        RefreshWalls();
    }
}
