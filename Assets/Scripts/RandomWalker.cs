using UnityEngine;

/// <summary>
/// Gère le déplacement aléatoire d’un PNJ dans un rayon donné autour de sa position d’origine.
/// Utilise le système de pathfinding via <see cref="PathfindingAI"/> pour se déplacer.
/// </summary>
public class RandomWalker : MonoBehaviour
{
    /// <summary>
    /// Rayon maximal autour de l’origine dans lequel le PNJ peut se déplacer.
    /// </summary>
    public float moveRadius = 10f;

    /// <summary>
    /// Temps d’attente entre chaque déplacement aléatoire.
    /// </summary>
    public float waitTime = 3f;

    private float timer;
    private Vector3 origin;
    private PathfindingAI pathfinding;

    /// <summary>
    /// Initialise la position d’origine et les références nécessaires.
    /// </summary>
    void Start()
    {
        pathfinding = GetComponent<PathfindingAI>();
        origin = transform.position;
        timer = waitTime;
    }

    /// <summary>
    /// À chaque frame, si aucune cible n’est définie et que le timer est écoulé, génère un nouveau point aléatoire à atteindre.
    /// </summary>
    void Update()
    {
        if (pathfinding.target == null && timer <= 0f)
        {
            Vector3 newPos = origin + new Vector3(
                Random.Range(-moveRadius, moveRadius),
                Random.Range(-moveRadius, moveRadius),
                0f
            );

            pathfinding.setTargetPos(newPos);
            timer = waitTime + Random.Range(0f, 2f); // Ajoute une variation
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
