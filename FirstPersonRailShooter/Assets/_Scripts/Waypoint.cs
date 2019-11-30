using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Inscribed")]
    public bool enemiesHiddenUntilReached = false;
    public List<Enemy> enemies;

    bool passThrough = true;
    bool waypointActive = false;
    bool waypointPositionReached = false;
    bool waypointRotationReached = false;
    bool hasEnemies = false;

    private void Awake()
    {
        // Make waypoint object invisible.
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rends)
            rend.enabled = false;
    }

    private void Start()
    {
        if (enemies.Count > 0)
        {
            hasEnemies = true;
            passThrough = false;
            // Store reference to waypoint in enemies.
            foreach (Enemy enemy in enemies)
            {
                enemy.myWaypoint = this;
                if (enemiesHiddenUntilReached)
                    enemy.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (waypointActive)
        {
            if (waypointPositionReached && waypointRotationReached)
            {
                if (passThrough || (hasEnemies && enemies.Count == 0))
                    PlayerMovement.IncrementWaypointIndex();
            }
        }
    }

    public void RemoveEnemy(Enemy _id)
    {
        if (!enemies.Remove(_id))
            Debug.LogError("Waypoint:RemoveEnemy(Enemy _id) - _id does not exist within list enemies.");
    }

    public void SetWaypointActive(bool _isActive)
    {
        waypointActive = _isActive;
        if (waypointActive)
        {

        }
        else
        {

        }
    }

    public void PositionReached()
    {
        waypointPositionReached = true;
        // If enemies are hidden, activate them.
        if (enemiesHiddenUntilReached)
            ActivateHiddenEnemies();
    }

    public void RotationReached()
    {
        waypointRotationReached = true;
    }

    void ActivateHiddenEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    public Vector3 Pos
    {
        get { return transform.position; }
    }

    public Vector3 Fwd
    {
        get { return transform.forward; }
    }

    public Vector3 LookAt
    {
        get { return (transform.position + transform.forward); }
    }

    public Quaternion Rot
    {
        get { return transform.rotation; }
    }
}
