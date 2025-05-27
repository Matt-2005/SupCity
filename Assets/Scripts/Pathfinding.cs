using UnityEngine;
using Pathfinding;

public class PathfindingAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 0.3f;

    private Path path;
    private int currentWayPoint = 0;

    private Seeker seeker;
    private Animator animator;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        animator = GetComponentInChildren<Animator>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
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
        if (path == null || target == null) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            animator?.SetBool("isMoving", false);
            GetComponent<BesoinPlayers>().NotifieArrivee();
            return;
        }

        Vector3 targetPosition = path.vectorPath[currentWayPoint];
        targetPosition.z = transform.position.z;

        Vector3 direction = (targetPosition - transform.position).normalized;

        // 🎞️ Animation
        if (animator != null)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.SetBool("isMoving", true);
        }

        // ✅ Mouvement constant
        Vector3 move = direction * speed * Time.deltaTime;
        transform.position += move;

        float distance = Vector3.Distance(transform.position, targetPosition);

        // Juste avant la fin du chemin → prépare la satisfaction
        if (currentWayPoint == path.vectorPath.Count - 1)
        {
            GetComponent<BesoinPlayers>().PreparerSatisfaction();
        }

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;

            if (currentWayPoint >= path.vectorPath.Count)
            {
                transform.position = targetPosition;
                animator?.SetBool("isMoving", false);
                GetComponent<BesoinPlayers>().NotifieArrivee();
            }
        }
    }

    public void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
