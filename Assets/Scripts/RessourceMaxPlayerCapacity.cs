using UnityEngine;

/// <summary>
/// Représente une ressource ayant une capacité maximale de joueurs pouvant l’utiliser en même temps.
/// Permet de réserver ou libérer dynamiquement l’accès à cette ressource.
/// </summary>
public class RessourceMaxPlayerCapacity : MonoBehaviour
{
    /// <summary>Nombre maximum de joueurs autorisés en même temps sur cette ressource.</summary>
    public int capaciteMax = 1;

    /// <summary>Nombre actuel de joueurs utilisant cette ressource.</summary>
    private int occupationActuelle = 0;

    public int OccupationActuelle
    {
        get { return occupationActuelle; }
    }

    /// <summary>
    /// Tente de réserver la ressource.
    /// Incrémente l’occupation si elle est disponible.
    /// </summary>
    /// <returns>True si la ressource a été réservée avec succès, false sinon.</returns>
    public bool VoirDisponibilite()
    {
        if (occupationActuelle < capaciteMax)
        {
            occupationActuelle++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Libère un emplacement d’occupation sur la ressource.
    /// </summary>
    public void Liberer()
    {
        occupationActuelle = Mathf.Max(0, occupationActuelle - 1);
    }

    /// <summary>
    /// Vérifie si la ressource est encore disponible sans réserver.
    /// </summary>
    /// <returns>True si la ressource est disponible, false sinon.</returns>
    public bool EstDisponible()
    {
        return occupationActuelle < capaciteMax;
    }
}
