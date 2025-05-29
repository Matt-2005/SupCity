using UnityEngine;

/// <summary>
/// Détruit automatiquement ce GameObject après un délai prédéfini.
/// Appelle la méthode <c>DéclencherDestruction</c> pour lancer la destruction après <c>delay</c> secondes.
/// </summary>
public class AutoDestroyAfterUse : MonoBehaviour
{
    /// <summary>
    /// Délai en secondes avant la destruction du GameObject.
    /// </summary>
    public float delay = 2f;

    /// <summary>
    /// Déclenche la destruction de ce GameObject après le délai défini.
    /// </summary>
    public void DéclencherDestruction()
    {
        Destroy(gameObject, delay);
    }
}
