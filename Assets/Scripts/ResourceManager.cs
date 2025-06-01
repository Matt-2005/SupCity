using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère les ressources du jeu : ajout, consommation, et accès global.
/// Utilise un pattern Singleton pour être accessible depuis n'importe quel script.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    /// <summary>
    /// Instance globale du ResourceManager (pattern Singleton).
    /// </summary>
    public static ResourceManager Instance;

    /// <summary>
    /// Dictionnaire contenant la quantité actuelle de chaque type de ressource.
    /// </summary>
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialise toutes les ressources à 0
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Ajoute une quantité donnée à une ressource.
    /// </summary>
    /// <param name="type">Le type de ressource à ajouter.</param>
    /// <param name="amount">La quantité à ajouter.</param>
    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
        Debug.Log($"[ResourceManager] +{amount} {type} (Total: {resources[type]})");
    }

    /// <summary>
    /// Tente de consommer une quantité de ressource. Échoue si la quantité est insuffisante.
    /// </summary>
    /// <param name="type">Le type de ressource à consommer.</param>
    /// <param name="amount">La quantité souhaitée.</param>
    /// <returns>True si la ressource a été consommée, False sinon.</returns>
    public bool ConsumeResource(ResourceType type, int amount)
    {
        if (resources.ContainsKey(type) && resources[type] >= amount)
        {
            resources[type] -= amount;
            Debug.Log($"[ResourceManager] -{amount} {type} (Remaining: {resources[type]})");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Récupère la quantité actuelle d'une ressource.
    /// </summary>
    /// <param name="type">Le type de ressource à interroger.</param>
    /// <returns>La quantité actuelle disponible.</returns>
    public int GetResource(ResourceType type)
    {
        return resources[type];
    }
}
