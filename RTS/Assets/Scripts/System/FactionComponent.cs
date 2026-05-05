using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { Player, Enemy }
public class FactionComponent : MonoBehaviour
{
    [Tooltip("진영 설정")]
    public Faction faction;
}
