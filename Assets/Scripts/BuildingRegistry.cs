using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère l'enregistrement centralisé des bâtiments selon leur tag.
/// Permet de retrouver dynamiquement tous les bâtiments d'un type donné (ex. : "Maison", "usineArgile", etc.).
/// Implémente le pattern Singleton via le champ statique <see cref="Instance"/>.
/// </summary>
public class BuildingRegistry : MonoBehaviour
{
    /// <summary>
    /// Instance unique du gestionnaire de bâtiments (Singleton).
    /// </summary>
    public static BuildingRegistry Instance;

    /// <summary>
    /// Dictionnaire contenant la liste des bâtiments par tag.
    /// Clé : tag du bâtiment, Valeur : liste des GameObjects correspondants.
    /// </summary>
    private Dictionary<string, List<GameObject>> batimentsParTag = new Dictionary<string, List<GameObject>>();

    /// <summary>
    /// Initialise l’instance du Singleton. Détruit le GameObject s’il existe déjà une instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Enregistre un bâtiment dans le dictionnaire selon son tag, s’il n’y est pas déjà.
    /// </summary>
    /// <param name="building">Le GameObject représentant le bâtiment à enregistrer.</param>
    public void RegisterBuilding(GameObject building)
    {
        string tag = building.tag;

        if (!batimentsParTag.ContainsKey(tag))
        {
            batimentsParTag[tag] = new List<GameObject>();
        }

        if (!batimentsParTag[tag].Contains(building))
        {
            batimentsParTag[tag].Add(building);
        }
    }

    /// <summary>
    /// Supprime un bâtiment du dictionnaire.
    /// </summary>
    /// <param name="building">Le GameObject représentant le bâtiment à retirer.</param>
    public void UnregisterBuilding(GameObject building)
    {
        string tag = building.tag;

        if (batimentsParTag.ContainsKey(tag))
        {
            batimentsParTag[tag].Remove(building);
        }
    }

    /// <summary>
    /// Récupère la liste des bâtiments associés à un tag donné.
    /// </summary>
    /// <param name="tag">Le tag des bâtiments recherchés.</param>
    /// <returns>Liste des GameObjects enregistrés avec ce tag. Liste vide si aucun trouvé.</returns>
    public List<GameObject> GetBuildingsWithTag(string tag)
    {
        if (batimentsParTag.ContainsKey(tag))
        {
            return batimentsParTag[tag];
        }

        return new List<GameObject>();
    }
}
