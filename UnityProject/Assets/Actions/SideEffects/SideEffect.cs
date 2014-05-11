using System;
using UnityEngine;

public abstract class SideEffect : MonoBehaviour
{
    public GameObject button;

    public abstract void additionalEffect(NodeData performr, Targetable target, bool isWin);
}
