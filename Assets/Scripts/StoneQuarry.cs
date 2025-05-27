using UnityEngine;
using System.Collections;

/// <summary>
/// Gère la production et le stockage de pierres dans une carrière.
/// La production se fait à intervalles réguliers tant que le stockage n’est pas plein.
/// Permet la collecte manuelle partielle ou totale.
/// </summary>
public class StoneQuarry : MonoBehaviour
{
    [Header("Production Settings")]
    /// <summary>Temps en secondes entre chaque cycle de production de pierre.</summary>
    [SerializeField] private float productionInterval = 5f;

    /// <summary>Quantité de pierre produite à chaque cycle.</summary>
    [SerializeField] private int productionAmount = 1;

    /// <summary>Capacité maximale de stockage de pierre.</summary>
    [SerializeField] private int maxStorageCapacity = 5000;

    [Header("References")]
    /// <summary>Prefab représentant la pierre produite (pour visualisation éventuelle).</summary>
    [SerializeField] private GameObject stonePrefab;

    /// <summary>Point de spawn des modèles visuels de pierre (optionnel).</summary>
    [SerializeField] private Transform stoneSpawnPoint;

    // Variables privées
    /// <summary>Quantité actuelle de pierre stockée.</summary>
    private int currentStoredStone = 0;

    /// <summary>Indique si la carrière est en train de produire.</summary>
    private bool isProducing = false;

    /// <summary>Coroutine de production active.</summary>
    private Coroutine productionCoroutine;

    /// <summary>Démarre la production automatiquement au lancement du jeu.</summary>
    private void Start()
    {
        StartProduction();
    }

    /// <summary>Redémarre la production si l’objet est réactivé.</summary>
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

    /// <summary>Démarre la coroutine de production de pierre.</summary>
    public void StartProduction()
    {
        if (!isProducing)
        {
            isProducing = true;
            productionCoroutine = StartCoroutine(ProduceStone());
            Debug.Log("La carrière commence à produire de la pierre.");
        }
    }

    /// <summary>Arrête la production de pierre si elle est active.</summary>
    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La carrière arrête de produire de la pierre.");
        }
    }

    /// <summary>Coroutine de production exécutée régulièrement selon le délai défini.</summary>
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

    /// <summary>
    /// Met à jour la visualisation de la pierre stockée (à implémenter selon besoin).
    /// Peut inclure la modification d’un modèle 3D ou d’une UI.
    /// </summary>
    private void UpdateStoneVisualization()
    {
        // Exemple: visualisation des pierres empilées selon la quantité
    }

    /// <summary>
    /// Collecte une quantité donnée de pierre (si disponible).
    /// </summary>
    /// <param name="amountToCollect">Quantité souhaitée</param>
    /// <returns>Quantité réellement collectée</returns>
    public int CollectStone(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredStone);
        currentStoredStone -= collectedAmount;

        Debug.Log($"Pierre collectée: {collectedAmount}. Restant: {currentStoredStone}");
        UpdateStoneVisualization();

        return collectedAmount;
    }

    /// <summary>Collecte toute la pierre actuellement stockée.</summary>
    /// <returns>Quantité totale collectée</returns>
    public int CollectAllStone()
    {
        int collectedAmount = currentStoredStone;
        currentStoredStone = 0;

        Debug.Log($"Toute la pierre collectée: {collectedAmount}");
        UpdateStoneVisualization();

        return collectedAmount;
    }

    /// <summary>Retourne la quantité de pierre stockée actuellement.</summary>
    public int GetStoredStoneAmount()
    {
        return currentStoredStone;
    }

    /// <summary>Indique si la capacité de stockage est atteinte.</summary>
    public bool IsStorageFull()
    {
        return currentStoredStone >= maxStorageCapacity;
    }

    /// <summary>Retourne le pourcentage d’utilisation du stockage (0 à 1).</summary>
    public float GetStoragePercentage()
    {
        return (float)currentStoredStone / maxStorageCapacity;
    }
}
