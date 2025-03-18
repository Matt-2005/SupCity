using UnityEngine;
using Pathfinding;

public class AI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();

        if (target == null)
        {
            Debug.LogError("Target not assigned.");
            return;
        }

        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector3 direction = ((Vector3)path.vectorPath[currentWayPoint] - transform.position).normalized;
        Vector3 move = direction * speed * Time.deltaTime;

        transform.position += move;

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }
    }
}