using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int minerals = 1000;

    public event Action OnMineralChanged;

    List<CommandCenter> commandCenters = new List<CommandCenter>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterCommandCenter(CommandCenter cc)
    {
        if (!commandCenters.Contains(cc))
            commandCenters.Add(cc);
    }

    public void UnregisterCommandCenter(CommandCenter cc)
    {
        if (commandCenters.Contains(cc))
            commandCenters.Remove(cc);
    }

    public CommandCenter GetNearestCommandCenter(Vector3 pos)
    {
        CommandCenter nearest = null;
        float minDist = float.MaxValue;
        foreach (var cc in commandCenters)
        {
            float d = Vector3.Distance(pos, cc.transform.position);
            if (d < minDist)
            {
                minDist = d;
                nearest = cc;
            }
        }
        return nearest;
    }

    public bool SpendResources(int amount)
    {
        if (minerals >= amount)
        {
            minerals -= amount;
            OnMineralChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void AddResources(int amount)
    {
        minerals += amount;
        OnMineralChanged?.Invoke();
    }
}
