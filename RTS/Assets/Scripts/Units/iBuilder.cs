using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder
{
    void OrderBuild(GameObject prefab, Vector3 placePos, Vector3 spawnPos);
}
