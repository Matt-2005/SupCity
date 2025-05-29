using UnityEngine;

/// <summary>
/// Gère les déplacements du joueur avec collision sur les objets solides.
/// Met à jour les paramètres de l'Animator en fonction des mouvements.
/// </summary>
public class ControlePlayer : MonoBehaviour
{
    /// <summary>Vitesse de déplacement du joueur.</summary>
    private float speed = 2.0f;

    /// <summary>Référence vers le composant Animator pour contrôler les animations.</summary>
    private Animator animator;

    /// <summary>LayerMask pour détecter les objets solides à ne pas traverser.</summary>
    public LayerMask solidObjectsLayer;

    /// <summary>Initialise la référence à l'Animator au démarrage.</summary>
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Gère les entrées clavier pour déplacer le joueur et mettre à jour l’animation.
    /// Empêche le déplacement si une collision est détectée avec un objet solide.
    /// </summary>
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 newPosition = transform.position + new Vector3(horizontal, vertical, 0);

        animator.SetFloat("moveX", horizontal);
        animator.SetFloat("moveY", vertical);
        animator.SetBool("isMoving", true);

        if (isWalkable(horizontal, vertical))
        {
            transform.position = newPosition;
        }
    }

    /// <summary>
    /// Vérifie si la position cible n’entre pas en collision avec un objet solide.
    /// </summary>
    /// <param name="x">Déplacement horizontal</param>
    /// <param name="y">Déplacement vertical</param>
    /// <returns>True si la case est libre, False sinon</returns>
    private bool isWalkable(float x, float y)
    {
        if (Physics2D.OverlapCircle(transform.position + new Vector3(x, y, 0), 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }
}
