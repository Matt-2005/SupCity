using UnityEngine;
using Pathfinding;

/// <summary>
/// Gère le déplacement automatique d’un PNJ vers une cible à l’aide d’A* Pathfinding.
/// Met à jour les animations et notifie l’arrivée au script BesoinPlayers.
/// </summary>
public class PathfindingAI : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float nextWayPointDistance = 0.3f;

    private Path path;
    private int currentWayPoint = 0;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            Vector3 startPosition = transform.position;

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            startPosition = rb.position;
        }
        else
        {
            Debug.LogWarning($"❌ {gameObject.name} n’a pas de Rigidbody2D, utilisation de transform.position.");
        }
        seeker.StartPath(startPosition, target.position, OnPathComplete);
        }
    }
    public void setTargetPos(Vector3 worldPosition)
    {
        GameObject tempTarget = new GameObject("TempTarget");
        tempTarget.transform.position = worldPosition;
        target = tempTarget.transform;
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
        if (target != null && target.name == "TargetTemp" && currentWayPoint >= path.vectorPath.Count)
        {
            Destroy(target.gameObject);
            target = null;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            animator?.SetBool("isMoving", false);
            GetComponent<BesoinPlayers>()?.NotifieArrivee();
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 movement = direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);

        if (animator != null)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        // Snap final pour précision
        if (currentWayPoint >= path.vectorPath.Count)
        {
            rb.position = path.vectorPath[^1];
            animator?.SetBool("isMoving", false);
            GetComponent<BesoinPlayers>()?.NotifieArrivee();
        }
    }

    /// <summary>Définit dynamiquement une nouvelle cible à atteindre.</summary>
    public void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
