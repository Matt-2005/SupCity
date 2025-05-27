using UnityEngine;
using System.Collections;

/// <summary>
/// Gère la production automatique de nourriture dans une cabane.
/// Produit de la nourriture à intervalles réguliers jusqu’à atteindre une capacité maximale.
/// Permet de collecter manuellement la nourriture stockée.
/// </summary>
public class FoodHut : MonoBehaviour
{
    [Header("Production Settings")]
    /// <summary>Temps entre deux productions de nourriture (en secondes).</summary>
    [SerializeField] private float productionInterval = 4f;

    /// <summary>Quantité de nourriture produite par cycle.</summary>
    [SerializeField] private int productionAmount = 3;

    /// <summary>Capacité maximale de stockage de nourriture.</summary>
    [SerializeField] private int maxStorageCapacity = 5000;

    [Header("References")]
    /// <summary>Prefab utilisé pour représenter visuellement la nourriture (optionnel).</summary>
    [SerializeField] private GameObject foodPrefab;

    // Variables privées
    /// <summary>Nourriture actuellement stockée dans la cabane.</summary>
    private int currentStoredFood = 0;

    /// <summary>Indique si la production est en cours.</summary>
    private bool isProducing = false;

    /// <summary>Référence à la coroutine de production.</summary>
    private Coroutine productionCoroutine;

    /// <summary>Démarre la production au lancement du jeu.</summary>
    private void Start()
    {
        StartProduction();
    }

    /// <summary>Redémarre la production si l'objet devient actif.</summary>
    private void OnEnable()
    {
        if (!isProducing)
        {
            StartProduction();
        }
    }

    /// <summary>Arrête la production si l'objet est désactivé.</summary>
    private void OnDisable()
    {
        StopProduction();
    }

    /// <summary>Active la coroutine de production si elle ne l’est pas déjà.</summary>
    public void StartProduction()
    {
        if (!isProducing)
        {
            isProducing = true;
            productionCoroutine = StartCoroutine(ProduceFood());
            Debug.Log("La cabane à nourriture commence à produire.");
        }
    }

    /// <summary>Arrête la coroutine de production si elle est active.</summary>
    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La cabane à nourriture arrête de produire.");
        }
    }

    /// <summary>Coroutine de production exécutée à intervalles réguliers.</summary>
    private IEnumerator ProduceFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            if (currentStoredFood < maxStorageCapacity)
            {
                int amountToAdd = Mathf.Min(productionAmount, maxStorageCapacity - currentStoredFood);
                currentStoredFood += amountToAdd;
                Debug.Log($"Nourriture produite: +{amountToAdd}. Total stocké: {currentStoredFood}/{maxStorageCapacity}");

                UpdateFoodVisualization();
            }
            else
            {
                Debug.Log("Le stockage est plein. La production est en pause.");
            }
        }
    }

    /// <summary>Met à jour visuellement la nourriture produite (placeholder à compléter).</summary>
    private void UpdateFoodVisualization()
    {
        // Exemple simple: On pourrait instantier un objet visuel pour représenter la nourriture
        // ou mettre à jour un compteur dans l'interface utilisateur
    }

    /// <summary>
    /// Permet de collecter une certaine quantité de nourriture si disponible.
    /// </summary>
    /// <param name="amountToCollect">Quantité souhaitée</param>
    /// <returns>Quantité effectivement collectée</returns>
    public int CollectFood(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredFood);
        currentStoredFood -= collectedAmount;

        Debug.Log($"Nourriture collectée: {collectedAmount}. Restant: {currentStoredFood}");
        UpdateFoodVisualization();

        return collectedAmount;
    }

    /// <summary>
    /// Permet de collecter toute la nourriture stockée.
    /// </summary>
    /// <returns>Quantité totale collectée</returns>
    public int CollectAllFood()
    {
        int collectedAmount = currentStoredFood;
        currentStoredFood = 0;

        Debug.Log($"Toute la nourriture collectée: {collectedAmount}");
        UpdateFoodVisualization();

        return collectedAmount;
    }

    /// <summary>Retourne la quantité de nourriture actuellement stockée.</summary>
    public int GetStoredFoodAmount()
    {
        return currentStoredFood;
    }
}
