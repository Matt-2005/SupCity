using UnityEngine;
using System.Collections;

/// <summary>
/// Gère la production automatique de bois dans une cabane de bûcheron.
/// Le bois est produit à intervalles réguliers jusqu'à remplir le stockage.
/// Permet la collecte partielle, totale ou l’ajout manuel.
/// </summary>
public class WoodcutterHut : MonoBehaviour
{
    [Header("Production Settings")]
    /// <summary>Temps entre deux cycles de production de bois (en secondes).</summary>
    [SerializeField] private float productionInterval = 5f;

    /// <summary>Quantité de bois produite à chaque cycle.</summary>
    [SerializeField] private int productionAmount = 5;

    /// <summary>Capacité maximale de stockage de bois.</summary>
    [SerializeField] private int maxStorageCapacity = 5000;

    [Header("References")]
    /// <summary>Prefab du bois (optionnel, pour la visualisation).</summary>
    [SerializeField] private GameObject woodPrefab;

    /// <summary>Point de stockage ou de visualisation des bûches (optionnel).</summary>
    [SerializeField] private Transform woodStorageArea;

    private int currentStoredWood = 0;
    private bool isProducing = false;
    private Coroutine productionCoroutine;

    /// <summary>Démarre automatiquement la production au lancement.</summary>
    private void Start()
    {
        StartProduction();
    }

    /// <summary>Relance la production si l’objet est activé.</summary>
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

    /// <summary>Lance la coroutine de production de bois.</summary>
    public void StartProduction()
    {
        if (!isProducing)
        {
            isProducing = true;
            productionCoroutine = StartCoroutine(ProduceWood());
            Debug.Log("La cabane du bûcheron commence à produire du bois.");
        }
    }

    /// <summary>Arrête la production de bois si elle est active.</summary>
    public void StopProduction()
    {
        if (isProducing && productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            isProducing = false;
            Debug.Log("La cabane du bûcheron arrête de produire du bois.");
        }
    }

    /// <summary>Coroutine qui gère la production régulière du bois.</summary>
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

    /// <summary>Met à jour la visualisation du bois stocké (ex : jauge, modèles 3D).</summary>
    private void UpdateWoodVisualization()
    {
        // Implémenter visualisation ici selon votre système
    }

    /// <summary>
    /// Collecte une quantité précise de bois.
    /// </summary>
    /// <param name="amountToCollect">Quantité demandée</param>
    /// <returns>Quantité effectivement collectée</returns>
    public int CollectWood(int amountToCollect)
    {
        int collectedAmount = Mathf.Min(amountToCollect, currentStoredWood);
        currentStoredWood -= collectedAmount;

        Debug.Log($"Bois collecté: {collectedAmount}. Restant: {currentStoredWood}");
        UpdateWoodVisualization();

        return collectedAmount;
    }

    /// <summary>
    /// Collecte tout le bois disponible.
    /// </summary>
    /// <returns>Quantité totale collectée</returns>
    public int CollectAllWood()
    {
        int collectedAmount = currentStoredWood;
        currentStoredWood = 0;

        Debug.Log($"Tout le bois collecté: {collectedAmount}");
        UpdateWoodVisualization();

        return collectedAmount;
    }

    /// <summary>Retourne la quantité actuelle de bois stockée.</summary>
    public int GetStoredWoodAmount()
    {
        return currentStoredWood;
    }

    /// <summary>Indique si le stockage est plein.</summary>
    public bool IsStorageFull()
    {
        return currentStoredWood >= maxStorageCapacity;
    }

    /// <summary>Retourne un pourcentage du remplissage du stockage.</summary>
    public float GetStoragePercentage()
    {
        return (float)currentStoredWood / maxStorageCapacity;
    }

    /// <summary>
    /// Ajoute manuellement du bois à la cabane (ex : récompense ou transfert).
    /// </summary>
    /// <param name="amountToAdd">Quantité à ajouter</param>
    /// <returns>Quantité réellement ajoutée (en fonction de l’espace disponible)</returns>
    public int AddWood(int amountToAdd)
    {
        int actuallyAdded = Mathf.Min(amountToAdd, maxStorageCapacity - currentStoredWood);
        currentStoredWood += actuallyAdded;

        Debug.Log($"Bois ajouté: {actuallyAdded}. Total actuel: {currentStoredWood}/{maxStorageCapacity}");
        UpdateWoodVisualization();

        return actuallyAdded;
    }
}
