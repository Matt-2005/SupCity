using UnityEngine;
using System.Collections;

/// <summary>
/// Gère la production automatique d'argile dans une cabane.
/// La production se fait à intervalle régulier tant que le stockage n'est pas plein.
/// Permet de collecter manuellement l'argile produite.
/// </summary>
public class ClayHut : MonoBehaviour
{
    [Header("Production Settings")]
    /// <summary>Temps (en secondes) entre chaque cycle de production.</summary>
    [SerializeField] private float productionInterval = 5f;

    /// <summary>Quantité d'argile produite à chaque cycle.</summary>
    [SerializeField] private int productionAmount = 2;

    /// <summary>Capacité maximale de stockage d'argile.</summary>
    [SerializeField] private int maxStorageCapacity = 3000;

    [Header("References")]
    /// <summary>Prefab utilisé pour représenter l'argile (optionnel si visualisation).</summary>
    [SerializeField] private GameObject clayPrefab;

    // Variables privées
    /// <summary>Quantité actuelle d'argile stockée.</summary>
    private int currentStoredClay = 0;

    /// <summary>Indique si la cabane est en train de produire.</summary>
    private bool isProducing = false;

    /// <summary>Coroutine en cours de production.</summary>
    private Coroutine productionCoroutine;

    /// <summary>Démarre automatiquement la production au lancement du jeu.</summary>
    private void Start()
    {
        StartProduction();
    }

    /// <summary>Relance la production si l’objet devient actif.</summary>
    private void OnEnable()
    {
        if (!isProducing)
        {
            StartProduction();
        }
    }

    /// <summary>Arrête la production si l’objet est désactivé.</summary>
    private void OnDisable()
    {
        StopProduction();
    }

    /// <summary>Lance la coroutine de production si elle n'est pas déjà active.</summary>
    public void StartProduction()
    {
        if (!isProducing)
        {
            isProducing = true;
            productionCoroutine = StartCoroutine(ProduceClay());
            Debug.Log("La cabane à argile commence à produire.");
        }
    }

    /// <summary>Arrête la coroutine de production si elle est active.</summary>
    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La cabane à argile arrête de produire.");
        }
    }

    /// <summary>Coroutine qui ajoute de l'argile à intervalles réguliers jusqu'à la capacité max.</summary>
    private IEnumerator ProduceClay()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);
            
            if (currentStoredClay < maxStorageCapacity)
            {
                int amountToAdd = Mathf.Min(productionAmount, maxStorageCapacity - currentStoredClay);
                currentStoredClay += amountToAdd;
                Debug.Log($"Argile produite: +{amountToAdd}. Total stocké: {currentStoredClay}/{maxStorageCapacity}");
                
                UpdateClayVisualization();
            }
            else
            {
                Debug.Log("Le stockage est plein. La production est en pause.");
            }
        }
    }

    /// <summary>Met à jour visuellement l’argile stockée (fonction vide pour extension future).</summary>
    private void UpdateClayVisualization()
    {
        // Ici vous pourriez implémenter une visualisation de l'argile stockée
    }

    /// <summary>
    /// Collecte une certaine quantité d’argile si elle est disponible.
    /// </summary>
    /// <param name="amountToCollect">Quantité demandée</param>
    /// <returns>Quantité réellement collectée</returns>
    public int CollectClay(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredClay);
        currentStoredClay -= collectedAmount;
        
        Debug.Log($"Argile collectée: {collectedAmount}. Restant: {currentStoredClay}");
        UpdateClayVisualization();
        
        return collectedAmount;
    }

    /// <summary>Collecte toute l’argile actuellement stockée.</summary>
    /// <returns>Quantité totale collectée</returns>
    public int CollectAllClay()
    {
        int collectedAmount = currentStoredClay;
        currentStoredClay = 0;
        
        Debug.Log($"Toute l'argile collectée: {collectedAmount}");
        UpdateClayVisualization();
        
        return collectedAmount;
    }

    /// <summary>Retourne la quantité d’argile actuellement en stock.</summary>
    public int GetStoredClayAmount()
    {
        return currentStoredClay;
    }
}
