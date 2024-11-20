using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] waypoints;
    

    private int _currentWaypointIndex = 0;


    private void Awake () {
    // Sets each child's parent to null while keeping their world position.
    foreach (Transform coord in waypoints) 
    {
        coord.SetParent(null, true);
    }
    }
    void Update()
    {
        // Move towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].position, speed * Time.deltaTime);

        // Check if we've reached the waypoint
        if (Vector3.Distance(transform.position, waypoints[_currentWaypointIndex].position) < 0.1f)
        {
            // Move to the next waypoint or loop back to the beginning
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
