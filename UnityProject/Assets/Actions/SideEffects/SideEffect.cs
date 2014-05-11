using System;
using UnityEngine;

public abstract class SideEffect : MonoBehaviour
{
    public GameObject button;
    public GameObject scheduledTag;

    public abstract void additionalEffect(NodeData performr, Targetable target, bool isWin);
}
