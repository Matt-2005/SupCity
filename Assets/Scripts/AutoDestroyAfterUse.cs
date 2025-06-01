using UnityEngine;

/// <summary>
/// Détruit automatiquement le GameObject après un délai donné lorsque la méthode est déclenchée.
/// </summary>
public class AutoDestroyAfterUse : MonoBehaviour
{
    /// <summary>
    /// Délai en secondes avant la destruction du GameObject après l'appel de <see cref="DéclencherDestruction"/>.
    /// </summary>
    public float delay = 2f;

    /// <summary>
    /// Déclenche la destruction du GameObject après le délai spécifié dans <see cref="delay"/>.
    /// </summary>
    public void DéclencherDestruction()
    {
        Destroy(gameObject, delay);
    }
}
