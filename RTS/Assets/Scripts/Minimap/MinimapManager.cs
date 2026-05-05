using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance;

    [Header("UI 설정")]
    public RectTransform minimapPanel;
    public GameObject iconPrefab;

    [Header("월드 크기 세팅")]
    public Vector2 worldSize;
    public Vector3 worldOrigin;

    Dictionary<MinimapEntity, MinimapIcon> icons = new Dictionary<MinimapEntity, MinimapIcon>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (var e in FindObjectsOfType<MinimapEntity>(true))
            if (!icons.ContainsKey(e))
                RegisterEntity(e);
    }

    void OnDisable()
    {
        icons.Clear();
    }

    public void RegisterEntity(MinimapEntity e)
    {
        if (icons.ContainsKey(e)) return;
        var go = Instantiate(iconPrefab, minimapPanel);
        var icon = go.GetComponent<MinimapIcon>();
        icon.SetTarget(e, minimapPanel.sizeDelta, worldSize, worldOrigin);
        icons[e] = icon;
    }

    public void UnregisterEntity(MinimapEntity e)
    {
        if (!icons.TryGetValue(e, out var icon)) return;
        Destroy(icon.gameObject);
        icons.Remove(e);
    }

    void Update()
    {
        if (!Application.isPlaying)
            return;

        var panelSize = minimapPanel.sizeDelta;
        var toRemove = new List<MinimapEntity>();

        foreach (var kv in icons)
        {
            var entity = kv.Key;
            var icon = kv.Value;

            if (icon == null || icon.gameObject == null)
            {
                toRemove.Add(entity);
                continue;
            }

            icon.UpdatePosition(panelSize, worldSize, worldOrigin);
        }

        foreach (var e in toRemove)
            icons.Remove(e);
    }
}