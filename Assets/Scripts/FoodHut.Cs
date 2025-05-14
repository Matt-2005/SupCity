using UnityEngine;
using System.Collections;

public class FoodHut : MonoBehaviour
{
    [Header("Production Settings")]
    [SerializeField] private float productionInterval = 5f; // Temps en secondes entre chaque production
    [SerializeField] private int productionAmount = 1; // Quantité produite à chaque cycle
    [SerializeField] private int maxStorageCapacity = 50; // Capacité maximale de stockage

    [Header("References")]
    [SerializeField] private GameObject foodPrefab; // Préfab pour la visualisation (optionnel)

    // Variables privées
    private int currentStoredFood = 0;
    private bool isProducing = false;
    private Coroutine productionCoroutine;

    // Variables pour l'interface utilisateur (à implémenter selon votre UI)
    private bool showUI = false;

    private void Start()
    {
        // Démarre la production automatiquement quand la cabane est placée
        StartProduction();
    }

    private void OnEnable()
    {
        // Redémarre la production si l'objet est réactivé
        if (!isProducing)
        {
            StartProduction();
        }
    }

    private void OnDisable()
    {
        // Arrête la production si l'objet est désactivé
        StopProduction();
    }

    // Méthode pour commencer la production
    public void StartProduction()
    {
        if (!isProducing)
        {
            isProducing = true;
            productionCoroutine = StartCoroutine(ProduceFood());
            Debug.Log("La cabane à nourriture commence à produire.");
        }
    }

    // Méthode pour arrêter la production
    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La cabane à nourriture arrête de produire.");
        }
    }

    // Coroutine pour produire de la nourriture à intervalle régulier
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
                
                // Option: Visualiser la nourriture produite
                UpdateFoodVisualization();
            }
            else
            {
                Debug.Log("Le stockage est plein. La production est en pause.");
            }
        }
    }

    // Méthode pour visualiser la nourriture (à adapter selon votre jeu)
    private void UpdateFoodVisualization()
    {
        // Exemple simple: On pourrait instantier un objet visuel pour représenter la nourriture
        // ou mettre à jour un compteur dans l'interface utilisateur
    }

    // Méthode pour récupérer de la nourriture
    public int CollectFood(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredFood);
        currentStoredFood -= collectedAmount;
        
        Debug.Log($"Nourriture collectée: {collectedAmount}. Restant: {currentStoredFood}");
        UpdateFoodVisualization();
        
        return collectedAmount;
    }

    // Méthode pour récupérer toute la nourriture
    public int CollectAllFood()
    {
        int collectedAmount = currentStoredFood;
        currentStoredFood = 0;
        
        Debug.Log($"Toute la nourriture collectée: {collectedAmount}");
        UpdateFoodVisualization();
        
        return collectedAmount;
    }

    // Permet de connaître la quantité de nourriture stockée
    public int GetStoredFoodAmount()
    {
        return currentStoredFood;
    }
}