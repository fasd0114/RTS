using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapEntity : MonoBehaviour
{
    public enum EntityType { SCV, Marine, Building, Mineral }
    public EntityType type;   

    void OnEnable() => MinimapManager.Instance?.RegisterEntity(this);
    void OnDisable() => MinimapManager.Instance?.UnregisterEntity(this);
}
