using UnityEngine;

/// <summary>
/// Met à jour en continu la position de la souris dans le monde (2D).
/// Stocke la position en temps réel dans une propriété statique accessible globalement.
/// </summary>
public class MousePosition : MonoBehaviour
{
    /// <summary>
    /// Position de la souris dans le monde (plan X/Y, z = 0).
    /// Accessible en lecture seule depuis n'importe quel script.
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
