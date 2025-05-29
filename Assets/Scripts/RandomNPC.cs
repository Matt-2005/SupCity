using UnityEngine;

/// <summary>
/// Fait se déplacer un PNJ aléatoirement dans une direction à intervalles irréguliers.
/// Évite les collisions avec les objets solides et anime les déplacements.
/// </summary>
public class RandomNPC : MonoBehaviour
{
    /// <summary>Vitesse de déplacement du PNJ.</summary>
    [SerializeField] private float speed = 2f;

    /// <summary>Liste des durées possibles avant un changement de direction.</summary>
    [SerializeField] private float[] moveTimeDurations = { 1f, 2f, 3f, 4f, 5f, 6f };

    /// <summary>Durée actuellement choisie pour le mouvement.</summary>
    [SerializeField] private float moveTime;

    /// <summary>Temps écoulé depuis le dernier changement de direction.</summary>
    [SerializeField] private float elapsedTime = 0f;

    private Vector2 currentDirection;
    private Vector3 startPosition;

    /// <summary>Référence à l'Animator pour gérer les animations de mouvement.</summary>
    private Animator animator;

    /// <summary>Layer contenant les objets solides à éviter (ex : murs, bâtiments).</summary>
    public LayerMask solidObjectsLayer;

    /// <summary>Initialise l'Animator et choisit une durée de mouvement aléatoire.</summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        moveTime = moveTimeDurations[Random.Range(0, moveTimeDurations.Length)];
    }

    /// <summary>Met à jour le comportement de déplacement aléatoire chaque frame.</summary>
    void Update()
    {
        moveRandomly();
    }

    /// <summary>Choisit une direction aléatoire (haut, bas, gauche, droite).</summary>
    void ChooseRandomDirection()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                currentDirection = Vector2.up;
                break;
            case 1:
                currentDirection = Vector2.down;
                break;
            case 2:
                currentDirection = Vector2.left;
                break;
            case 3:
                currentDirection = Vector2.right;
                break;
        }
    }

    /// <summary>
    /// Gère le déplacement continu du PNJ selon la direction choisie,
    /// change de direction lorsque la durée définie est atteinte.
    /// </summary>
    void moveRandomly()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= moveTime)
        {
            elapsedTime = 0f;
            ChooseRandomDirection();
        }

        float horizontal = currentDirection.x * speed * Time.deltaTime;
        float vertical = currentDirection.y * speed * Time.deltaTime;

        animator.SetFloat("moveX", currentDirection.x);
        animator.SetFloat("moveY", currentDirection.y);

        if (isWalkable(horizontal, vertical))
        {
            animator.SetBool("isMoving", true);
            transform.Translate(new Vector3(horizontal, vertical, 0));
        }
    }

    /// <summary>
    /// Vérifie si le déplacement dans la direction souhaitée est possible (pas d'obstacle).
    /// </summary>
    /// <param name="x">Déplacement horizontal</param>
    /// <param name="y">Déplacement vertical</param>
    /// <returns>True si la zone est libre, sinon false</returns>
    private bool isWalkable(float x, float y)
    {
        if (Physics2D.OverlapCircle(transform.position + new Vector3(x, y, 0), 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }
}
