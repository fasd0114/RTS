using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class WallCarver : MonoBehaviour
{
    void Awake()
    {
        var obst = GetComponent<NavMeshObstacle>();
        obst.carving = true;
        obst.shape = NavMeshObstacleShape.Box;
        var col = GetComponent<Collider>();
        if (col != null)
            obst.size = col.bounds.size;
    }
}
