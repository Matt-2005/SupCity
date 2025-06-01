using UnityEngine;
using Pathfinding;

/// <summary>
/// Gère le déplacement automatique d’un PNJ vers une cible à l’aide d’A* Pathfinding.
/// Met à jour les animations et notifie l’arrivée au script <see cref="BesoinPlayers"/>.
/// </summary>
public class PathfindingAI : MonoBehaviour
{
    /// <summary>Transform du GameObject cible à atteindre.</summary>
    public Transform target;

    /// <summary>Vitesse de déplacement du PNJ.</summary>
    public float speed = 3f;

    /// <summary>Distance minimale pour considérer qu’un waypoint a été atteint.</summary>
    public float nextWayPointDistance = 0.3f;

    private Path path;
    private int currentWayPoint = 0;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;

    /// <summary>Indique si le script a déjà notifié l’arrivée au besoin.</summary>
    private bool aNotifieArrivee = false;

    /// <summary>Initialise les composants et lance la mise à jour régulière du chemin.</summary>
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    /// <summary>Met à jour le chemin vers la cible à l’aide de A* Pathfinding.</summary>
    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            Vector3 startPosition = rb != null ? rb.position : transform.position;
            seeker.StartPath(startPosition, target.position, OnPathComplete);
        }
    }

    /// <summary>Callback appelé une fois le chemin calculé avec succès.</summary>
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
            aNotifieArrivee = false;
        }
    }

    /// <summary>
    /// Gère le déplacement du PNJ à chaque frame physique.
    /// Suit les waypoints du chemin, met à jour les animations et notifie l’arrivée.
    /// </summary>
    void FixedUpdate()
    {
        if (path == null || target == null) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (!aNotifieArrivee)
            {
                aNotifieArrivee = true;
                animator?.SetBool("isMoving", false);

                if (target.name.StartsWith("TempTarget") || target.name.StartsWith("TargetRelax"))
                {
                    Destroy(target.gameObject);
                    target = null;
                }

                GetComponent<BesoinPlayers>()?.NotifieArrivee();
            }
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

        if (currentWayPoint >= path.vectorPath.Count && !aNotifieArrivee)
        {
            rb.position = path.vectorPath[^1];
            aNotifieArrivee = true;
            animator?.SetBool("isMoving", false);

            if (target.name.StartsWith("TempTarget") || target.name.StartsWith("TargetRelax"))
            {
                Destroy(target.gameObject);
                target = null;
            }

            GetComponent<BesoinPlayers>()?.NotifieArrivee();
        }
    }

    /// <summary>
    /// Définit dynamiquement une nouvelle cible à atteindre.
    /// Réinitialise la notification d’arrivée.
    /// </summary>
    /// <param name="newTarget">Transform de la nouvelle cible.</param>
    public void setTarget(Transform newTarget)
    {
        target = newTarget;
        aNotifieArrivee = false;
    }

    /// <summary>
    /// Crée une cible temporaire à une position donnée et s'y rend.
    /// </summary>
    /// <param name="worldPosition">Position dans le monde vers laquelle se déplacer.</param>
    public void setTargetPos(Vector3 worldPosition)
    {
        GameObject tempTarget = new GameObject("TempTarget");
        tempTarget.transform.position = worldPosition;
        setTarget(tempTarget.transform);
    }
}
