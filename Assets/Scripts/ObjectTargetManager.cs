using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère la liste des objets cibles disponibles dans la scène (ex. : arbres).
/// Initialise automatiquement la liste en recherchant les GameObjects ayant le tag "Arbre".
/// </summary>
public class ObjectTargetManager : MonoBehaviour
{
    /// <summary>
    /// Liste des arbres actuellement disponibles dans la scène.
    /// </summary>
    public List<GameObject> arbresDisponibles = new List<GameObject>();

    /// <summary>
    /// Initialise la liste des arbres en les trouvant via leur tag "Arbre".
    /// </summary>
    void Start()
    {
        GameObject[] arbres = GameObject.FindGameObjectsWithTag("Arbre");

        foreach (GameObject arbre in arbres)
        {
            arbresDisponibles.Add(arbre);
        }
    }

    /// <summary>
    /// Méthode appelée à chaque frame (actuellement vide).
    /// </summary>
    void Update()
    {
        
    }
}
