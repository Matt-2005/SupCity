using UnityEngine;

/// <summary>
/// Gère la capacité d'occupation d'un objet (ex. : bâtiment, ressource).
/// Permet de vérifier si de la place est disponible, de réserver ou libérer une place.
/// </summary>
public class PlayerCapacity : MonoBehaviour
{
    /// <summary>
    /// Nombre maximal d’occupants autorisés.
    /// </summary>
    public int maxOccupants = 1;

    /// <summary>
    /// Nombre actuel d’occupants.
    /// </summary>
    private int currentOccupants = 0;

    /// <summary>
    /// Vérifie s'il reste une place disponible et réserve immédiatement une place si oui.
    /// </summary>
    /// <returns>True si une place a été réservée, sinon False.</returns>
    public bool VoirDisponibilite()
    {
        if (currentOccupants < maxOccupants)
        {
            currentOccupants++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Libère une place. Le nombre d’occupants ne descend jamais en dessous de 0.
    /// </summary>
    public void Liberer()
    {
        currentOccupants = Mathf.Max(0, currentOccupants - 1);
    }

    /// <summary>
    /// Indique simplement si une place est disponible, sans réserver.
    /// </summary>
    /// <returns>True si la capacité n’est pas encore atteinte.</returns>
    public bool EstDisponible()
    {
        return currentOccupants < maxOccupants;
    }
}
