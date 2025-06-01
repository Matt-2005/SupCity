using UnityEngine;

/// <summary>
/// Met à jour en temps réel la position de la souris dans le monde 2D.
/// La position est accessible globalement via <see cref="MouseWorldPosition"/>.
/// </summary>
public class MousePosition : MonoBehaviour
{
    /// <summary>
    /// Position de la souris en coordonnées du monde (z = 0 pour la 2D).
    /// Accessible globalement en lecture seule.
    /// </summary>
    public static Vector3 MouseWorldPosition { get; private set; }

    /// <summary>
    /// Met à jour la position de la souris dans le monde à chaque frame.
    /// </summary>
    void Update()
    {
        Vector3 tempPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPosition.z = 0f;
        MouseWorldPosition = tempPosition;
    }
}
