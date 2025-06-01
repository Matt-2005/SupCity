using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère la liste des objets cibles disponibles portant le tag "usineBaie".
/// Utile pour accéder rapidement aux ressources de type baie dans la scène.
/// </summary>
public class ObjectTargetManager : MonoBehaviour
{
    /// <summary>
    /// Liste des GameObjects disponibles avec le tag "usineBaie".
    /// </summary>
    public List<GameObject> baieDisponibles = new List<GameObject>();

    /// <summary>
    /// Initialise la liste <see cref="baieDisponibles"/> en trouvant tous les objets avec le tag "usineBaie" dans la scène.
    /// </summary>
    void Start()
    {
        GameObject[] baies = GameObject.FindGameObjectsWithTag("usineBaie");

        foreach (GameObject baie in baies)
        {
            baieDisponibles.Add(baie);
        }
    }
}
