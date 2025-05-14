using UnityEngine;
using System.Collections;

public class WoodcutterHut : MonoBehaviour
{
    [Header("Production Settings")]
    [SerializeField] private float productionInterval = 5f; 
    [SerializeField] private int productionAmount = 5; 
    [SerializeField] private int maxStorageCapacity = 5000; 

    [Header("References")]
    [SerializeField] private GameObject woodPrefab; 
    [SerializeField] private Transform woodStorageArea;

    private int currentStoredWood = 0;
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
            productionCoroutine = StartCoroutine(ProduceWood());
            Debug.Log("La cabane du bûcheron commence à produire du bois.");
        }
    }

    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La cabane du bûcheron arrête de produire du bois.");
        }
    }

    private IEnumerator ProduceWood()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);
            
            if (currentStoredWood < maxStorageCapacity)
            {
                int amountToAdd = Mathf.Min(productionAmount, maxStorageCapacity - currentStoredWood);
                currentStoredWood += amountToAdd;
                Debug.Log($"Bois produit: +{amountToAdd}. Total stocké: {currentStoredWood}/{maxStorageCapacity}");
                
                UpdateWoodVisualization();
            }
            else
            {
                Debug.Log("Le stockage de bois est plein. La production est en pause.");
            }
        }
    }

    private void UpdateWoodVisualization()
    {
        // Exemple: Vous pourriez faire apparaître des modèles 3D de bûches
        // à côté de la cabane, proportionnellement à la quantité stockée
        
        // Vous pourriez aussi mettre à jour un compteur ou une jauge visuelle
        // qui indique la quantité de bois disponible
    }

    public int CollectWood(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredWood);
        currentStoredWood -= collectedAmount;
        
        Debug.Log($"Bois collecté: {collectedAmount}. Restant: {currentStoredWood}");
        UpdateWoodVisualization();
        
        return collectedAmount;
    }

    public int CollectAllWood()
    {
        int collectedAmount = currentStoredWood;
        currentStoredWood = 0;
        
        Debug.Log($"Tout le bois collecté: {collectedAmount}");
        UpdateWoodVisualization();
        
        return collectedAmount;
    }

    public int GetStoredWoodAmount()
    {
        return currentStoredWood;
    }
    
    public bool IsStorageFull()
    {
        return currentStoredWood >= maxStorageCapacity;
    }
    
    public float GetStoragePercentage()
    {
        return (float)currentStoredWood / maxStorageCapacity;
    }
    
    public int AddWood(int amountToAdd)
    {
        int actuallyAdded = Mathf.Min(amountToAdd, maxStorageCapacity - currentStoredWood);
        currentStoredWood += actuallyAdded;
        
        Debug.Log($"Bois ajouté: {actuallyAdded}. Total actuel: {currentStoredWood}/{maxStorageCapacity}");
        UpdateWoodVisualization();
        
        return actuallyAdded;
    }
}