using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newInactiveState", menuName = "Data/State Data/Inactive State")]
public class D_InactiveState : ScriptableObject
{
    public float activeRadius = 0.5f;

    public LayerMask whatIsPlayer;
}
