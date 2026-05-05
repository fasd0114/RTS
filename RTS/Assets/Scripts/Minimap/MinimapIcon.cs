using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class MinimapIcon : MonoBehaviour
{
    RectTransform rt;
    Image img;
    MinimapEntity entity;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
    }
    public void SetTarget(MinimapEntity e, Vector2 panelSize, Vector2 worldSize, Vector3 worldOrigin)
    {
        entity = e;

        switch (e.type)
        {
            case MinimapEntity.EntityType.Mineral:
                img.color = Color.blue;
                break;

            case MinimapEntity.EntityType.SCV:
            case MinimapEntity.EntityType.Marine:
            case MinimapEntity.EntityType.Building:
                var fac = e.GetComponent<FactionComponent>();
                if (fac != null && fac.faction == Faction.Player)
                    img.color = Color.green;
                else
                    img.color = Color.red;
                break;
        }
    }

    public void UpdatePosition(Vector2 panelSize, Vector2 worldSize, Vector3 worldOrigin)
    {
        Vector3 wp = entity.transform.position;
        float u = (wp.x - worldOrigin.x) / worldSize.x;
        float v = (wp.z - worldOrigin.z) / worldSize.y;

        float x = u * panelSize.x - panelSize.x * 0.5f;
        float y = v * panelSize.y - panelSize.y * 0.5f;
        rt.anchoredPosition = new Vector2(x, y);
    }
}
