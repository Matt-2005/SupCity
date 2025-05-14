using UnityEngine;
using System.Collections;

public class StoneQuarry : MonoBehaviour
{
    [Header("Production Settings")]
    [SerializeField] private float productionInterval = 5f;
    [SerializeField] private int productionAmount = 1; 
    [SerializeField] private int maxStorageCapacity = 5000;

    [Header("References")]
    [SerializeField] private GameObject stonePrefab; 
    [SerializeField] private Transform stoneSpawnPoint; 

    // Variables privées
    private int currentStoredStone = 0;
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
            productionCoroutine = StartCoroutine(ProduceStone());
            Debug.Log("La carrière commence à produire de la pierre.");
        }
    }


    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La carrière arrête de produire de la pierre.");
        }
    }

    private IEnumerator ProduceStone()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);
            
            if (currentStoredStone < maxStorageCapacity)
            {
                int amountToAdd = Mathf.Min(productionAmount, maxStorageCapacity - currentStoredStone);
                currentStoredStone += amountToAdd;
                Debug.Log($"Pierre produite: +{amountToAdd}. Total stocké: {currentStoredStone}/{maxStorageCapacity}");
                
          
                UpdateStoneVisualization();
            }
            else
            {
                Debug.Log("Le stockage de pierre est plein. La production est en pause.");
            }
        }
    }


    private void UpdateStoneVisualization()
    {
        // Exemple: Vous pourriez ajouter ici du code pour faire apparaître 
        // des modèles 3D de pierres empilées à côté de la carrière
        // Plus il y a de pierres stockées, plus la pile est grande
        
        // Ou vous pourriez simplement mettre à jour une variable qui sera 
        // utilisée par un autre système pour afficher le nombre de pierres stockées
    }

    public int CollectStone(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredStone);
        currentStoredStone -= collectedAmount;
        
        Debug.Log($"Pierre collectée: {collectedAmount}. Restant: {currentStoredStone}");
        UpdateStoneVisualization();
        
        return collectedAmount;
    }

    public int CollectAllStone()
    {
        int collectedAmount = currentStoredStone;
        currentStoredStone = 0;
        
        Debug.Log($"Toute la pierre collectée: {collectedAmount}");
        UpdateStoneVisualization();
        
        return collectedAmount;
    }

    public int GetStoredStoneAmount()
    {
        return currentStoredStone;
    }

    public bool IsStorageFull()
    {
        return currentStoredStone >= maxStorageCapacity;
    }

    public float GetStoragePercentage()
    {
        return (float)currentStoredStone / maxStorageCapacity;
    }
}