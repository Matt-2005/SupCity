using UnityEngine;
using Pathfinding;

public class PathfindingAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    public LayerMask solidObjectsLayer;

    void Start()
    {
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
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
        if (path == null) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            GetComponent<BesoinPlayers>().NotifieArrivee();
            return;
        }

        reachedEndOfPath = false;

        Vector3 targetPosition = path.vectorPath[currentWayPoint];
        targetPosition.z = transform.position.z;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance < 0.05f)
        {
            currentWayPoint++;
        }
    }

    public void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
