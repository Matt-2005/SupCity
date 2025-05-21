using UnityEngine;
using System.Collections;

public class ClayHut : MonoBehaviour
{
    [Header("Production Settings")]
    [SerializeField] private float productionInterval = 5f; 
    [SerializeField] private int productionAmount = 2;
    [SerializeField] private int maxStorageCapacity = 3000;

    [Header("References")]
    [SerializeField] private GameObject clayPrefab;

    // Variables privées
    private int currentStoredClay = 0;
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
            productionCoroutine = StartCoroutine(ProduceClay());
            Debug.Log("La cabane à argile commence à produire.");
        }
    }

    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La cabane à argile arrête de produire.");
        }
    }

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

    private void UpdateClayVisualization()
    {
        // Ici vous pourriez implémenter une visualisation de l'argile stockée
        // Par exemple changer un modèle 3D ou instantier des objets pour représenter l'argile
    }

    public int CollectClay(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredClay);
        currentStoredClay -= collectedAmount;
        
        Debug.Log($"Argile collectée: {collectedAmount}. Restant: {currentStoredClay}");
        UpdateClayVisualization();
        
        return collectedAmount;
    }

    public int CollectAllClay()
    {
        int collectedAmount = currentStoredClay;
        currentStoredClay = 0;
        
        Debug.Log($"Toute l'argile collectée: {collectedAmount}");
        UpdateClayVisualization();
        
        return collectedAmount;
    }

    public int GetStoredClayAmount()
    {
        return currentStoredClay;
    }
}