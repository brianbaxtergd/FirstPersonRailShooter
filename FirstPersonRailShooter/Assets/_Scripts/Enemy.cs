using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Dynamic")]
    public Waypoint myWaypoint;

    private void OnDestroy()
    {
        if (myWaypoint != null)
            myWaypoint.RemoveEnemy(this);
    }
}
