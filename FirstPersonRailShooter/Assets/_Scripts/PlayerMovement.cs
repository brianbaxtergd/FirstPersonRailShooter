using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    static private PlayerMovement _S;

    public enum ePlayerMovementStates
    {
        idle,
        moving,
        turning,
    }

    [Header("Inscribed")]
    public Waypoint[] waypoints;
    public float moveSpeedMax;
    public float moveSmoothTime;
    public float turnTime;

    [Header("Dynamic")]
    [SerializeField] ePlayerMovementStates state;
    [SerializeField] int currWaypointIndex = 0;

    bool currWaypointIsEnd = false;
    float turnTimer = 0f;
    Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        _S = this;
    }

    void Start()
    {
        state = ePlayerMovementStates.moving;

        // Activate starting waypoint.
        waypoints[0].SetWaypointActive(true);
    }

    void Update()
    {
        switch (state)
        {
            case ePlayerMovementStates.idle:
                // Wait for the current active waypoint to increment the waypoint index.
                break;
            case ePlayerMovementStates.moving:
                // Move towards current waypoint position.
                //transform.position = Vector3.MoveTowards(transform.position, waypoints[currWaypointIndex].transform.position, moveSpeed * Time.deltaTime);
                transform.position = Vector3.SmoothDamp(transform.position, waypoints[currWaypointIndex].Pos, ref currentVelocity, moveSmoothTime, moveSpeedMax);
                // Check if waypoint has been reached.
                if (Vector3.Distance(transform.position, waypoints[currWaypointIndex].transform.position) <= 0.01f)
                {
                    // Snap to waypoint position and switch to turning state.
                    transform.position = waypoints[currWaypointIndex].transform.position;
                    state = ePlayerMovementStates.turning;
                    waypoints[currWaypointIndex].PositionReached();
                }
                break;
            case ePlayerMovementStates.turning:
                // Turn timer tick.
                turnTimer += Time.deltaTime;
                // Rotate towards current waypoint rotation.
                transform.rotation = Quaternion.Slerp(transform.rotation, waypoints[currWaypointIndex].transform.rotation, turnTimer/turnTime);
                // Check if rotation is complete.
                if (Quaternion.Angle(transform.rotation, waypoints[currWaypointIndex].transform.rotation) <= 0.01f)
                {
                    // Reset turn timer.
                    turnTimer = 0f;
                    // Snap to waypoint rotation and switch to idle state.
                    transform.rotation = waypoints[currWaypointIndex].transform.rotation;
                    state = ePlayerMovementStates.idle;
                    waypoints[currWaypointIndex].RotationReached();
                }
                break;
            default:
                break;
        }
    }

    // Statics.

    static public PlayerMovement S
    {
        get { return _S; }
        private set
        {
            if (_S != null)
                Debug.LogError("PlayerMovement:S - Multiple assignments to singleton _S.");
            _S = value;
        }
    }

    static public void IncrementWaypointIndex()
    {
        if (S.currWaypointIsEnd)
            return;

        // Increment waypoint index.
        S.currWaypointIndex++;

        // Activate current waypoint and deactivate previous waypoint.
        if (S.currWaypointIndex < S.waypoints.Length)
            S.waypoints[S.currWaypointIndex].SetWaypointActive(true);
        S.waypoints[S.currWaypointIndex - 1].SetWaypointActive(false);

        // Switch to moving state.
        S.state = ePlayerMovementStates.moving;

        // Check if new waypoint is the final waypoint.
        if (S.currWaypointIndex == S.waypoints.Length - 1)
        {
            S.currWaypointIsEnd = true;
        }
    }
}
