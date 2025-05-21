using UnityEngine;
using System.Collections;

public class FoodHut : MonoBehaviour
{
    [Header("Production Settings")]
    [SerializeField] private float productionInterval = 4f;
    [SerializeField] private int productionAmount = 3;
    [SerializeField] private int maxStorageCapacity = 5000;

    [Header("References")]
    [SerializeField] private GameObject foodPrefab;

    // Variables privées
    private int currentStoredFood = 0;
    private bool isProducing = false;
    private Coroutine productionCoroutine;

    private void Start()
    {
        StartProduction();
    }

    private void OnEnable()
    {
        if (!isProducing)
        {
            StartProduction();
        }
    }

    private void OnDisable()
    {
        StopProduction();
    }

    public void StartProduction()
    {
        if (!isProducing)
        {
            isProducing = true;
            productionCoroutine = StartCoroutine(ProduceFood());
            Debug.Log("La cabane à nourriture commence à produire.");
        }
    }

    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La cabane à nourriture arrête de produire.");
        }
    }

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

    private void UpdateFoodVisualization()
    {
        // Exemple simple: On pourrait instantier un objet visuel pour représenter la nourriture
        // ou mettre à jour un compteur dans l'interface utilisateur
    }


    public int CollectFood(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredFood);
        currentStoredFood -= collectedAmount;
        
        Debug.Log($"Nourriture collectée: {collectedAmount}. Restant: {currentStoredFood}");
        UpdateFoodVisualization();
        
        return collectedAmount;
    }


    public int CollectAllFood()
    {
        int collectedAmount = currentStoredFood;
        currentStoredFood = 0;
        
        Debug.Log($"Toute la nourriture collectée: {collectedAmount}");
        UpdateFoodVisualization();
        
        return collectedAmount;
    }

    public int GetStoredFoodAmount()
    {
        return currentStoredFood;
    }
}