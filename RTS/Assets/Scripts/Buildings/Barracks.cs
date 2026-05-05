using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    public GameObject marinePrefab;
    public Transform spawnPoint;
    public int marineCost = 100;

    void OnMouseDown()
    {
        UnitProductionUI.Instance.ShowBarracks(this);
    }

    public void ProduceMarine()
    {
        if (GameManager.Instance.SpendResources(marineCost))
        {
            Instantiate(marinePrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}

