using UnityEngine;
using Pathfinding;

/// <summary>
/// Gère le déplacement automatique d’un PNJ vers une cible en utilisant le pathfinding A*.
/// Met à jour les animations selon la direction et notifie l'arrivée via le script <c>BesoinPlayers</c>.
/// </summary>
public class PathfindingAI : MonoBehaviour
{
    /// <summary>Transform de la cible à atteindre.</summary>
    public Transform target;

    /// <summary>Vitesse de déplacement du PNJ.</summary>
    public float speed = 200f;

    /// <summary>Distance minimale pour passer au waypoint suivant.</summary>
    public float nextWayPointDistance = 0.3f;

    private Path path;
    private int currentWayPoint = 0;

    private Seeker seeker;

    /// <summary>Layer contenant les objets solides (non traversables).</summary>
    public LayerMask solidObjectsLayer;

    /// <summary>Référence à l’Animator (sur l’enfant du PNJ).</summary>
    private Animator animator;

    /// <summary>Initialise le seeker et démarre la mise à jour du chemin à intervalle régulier.</summary>
    void Start()
    {
        seeker = GetComponent<Seeker>();
        animator = GetComponentInChildren<Animator>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    /// <summary>Demande au Seeker de recalculer un chemin vers la cible.</summary>
    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    /// <summary>Callback appelée lorsque le chemin est calculé avec succès.</summary>
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    /// <summary>Déplace le PNJ frame par frame en suivant le chemin calculé.</summary>
    void FixedUpdate()
    {
        if (path == null || target == null) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            // Fin du chemin déjà atteinte
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }

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

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;

            // 📌 Si c’est le dernier point qu’on atteint, snap direct pour éviter le ralentissement
            if (currentWayPoint >= path.vectorPath.Count)
            {
                transform.position = targetPosition;

                if (animator != null)
                {
                    animator.SetBool("isMoving", false);
                }

                GetComponent<BesoinPlayers>().NotifieArrivee();
            }
        }
    }

    /// <summary>
    /// Définit dynamiquement une nouvelle cible à atteindre.
    /// </summary>
    /// <param name="newTarget">Transform de la nouvelle cible</param>
    public void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
