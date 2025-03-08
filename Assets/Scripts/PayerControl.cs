using UnityEngine;

public class CharacterMoveRandom : MonoBehaviour
{
    public float speed = 2f; // Vitesse de déplacement
    public float moveTime = 2f; // Temps avant changement de direction
    public float returnTime = 10f; // Temps avant de revenir au point d'origine

    private float elapsedTime = 0f;
    private float returnElapsedTime = 0f;
    private Vector2 currentDirection;
    private Vector3 startPosition; // Position d'origine
    private bool returning = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position; // Sauvegarde la position d'origine
        ChooseRandomDirection();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        returnElapsedTime += Time.deltaTime;

        if (returnElapsedTime >= returnTime)
        {
            // On active le retour à la position d'origine
            returning = true;
        }

        if (returning)
        {
            ReturnToStart();
        }
        else
        {
            MoveRandomly();
        }
    }

    void MoveRandomly()
    {
        // Changer de direction après `moveTime` secondes
        if (elapsedTime >= moveTime)
        {
            elapsedTime = 0f; // Réinitialiser le timer
            ChooseRandomDirection(); // Nouvelle direction aléatoire
        }

        // Calcul du déplacement
        float horizontal = currentDirection.x * speed * Time.deltaTime;
        float vertical = currentDirection.y * speed * Time.deltaTime;

        // Mettre à jour l'animation
        animator.SetFloat("moveX", currentDirection.x);
        animator.SetFloat("moveY", currentDirection.y);
        animator.SetBool("isMoving", true);

        // Appliquer le déplacement
        transform.position += new Vector3(horizontal, vertical, 0);
    }

    void ReturnToStart()
    {
        // Déplacement progressif vers la position d'origine
        transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

        // Vérifie si on est revenu à la position d'origine
        if (Vector3.Distance(transform.position, startPosition) < 0.1f)
        {
            returning = false; // Fin du retour
            returnElapsedTime = 0f; // Reset du timer
            ChooseRandomDirection(); // Reprendre les mouvements aléatoires
        }
    }

    void ChooseRandomDirection()
    {
        // Liste des directions possibles
        Vector2[] possibleDirections = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

        // Choisir une direction aléatoire
        currentDirection = possibleDirections[Random.Range(0, possibleDirections.Length)];
    }
}
