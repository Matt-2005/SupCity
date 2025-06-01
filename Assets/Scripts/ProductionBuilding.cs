using UnityEngine;

/// <summary>
/// Classe abstraite pour les bâtiments de production.
/// Gère la logique de production cyclique avec ressources en entrée optionnelles.
/// À hériter pour définir des bâtiments spécifiques (ex : usine de briques, champ de baies...).
/// </summary>
public abstract class ProductionBuilding : MonoBehaviour
{
    [Header("Production Settings")]

    /// <summary>
    /// Type de ressource produite par ce bâtiment.
    /// </summary>
    public ResourceType outputResourceType;

    /// <summary>
    /// Quantité de ressource produite à chaque cycle.
    /// </summary>
    public int productionAmount = 1;

    /// <summary>
    /// Intervalle (en secondes) entre deux productions.
    /// </summary>
    public float productionInterval = 5f;

    /// <summary>
    /// Type de ressource consommée pour produire (null si aucune ressource requise).
    /// Peut être redéfini dans une classe dérivée.
    /// </summary>
    protected virtual ResourceType? inputResourceType => null;

    /// <summary>
    /// Quantité de ressource en entrée nécessaire à chaque cycle (0 si aucune).
    /// Peut être redéfini dans une classe dérivée.
    /// </summary>
    protected virtual int inputAmount => 0;

    private float timer;

    /// <summary>
    /// Initialise le timer de production à l’intervalle défini.
    /// </summary>
    protected virtual void Start()
    {
        timer = productionInterval;
    }

    /// <summary>
    /// Décrémente le timer et déclenche la production à intervalle régulier.
    /// </summary>
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Produce();
            timer = productionInterval;
        }
    }

    /// <summary>
    /// Méthode de production exécutée à chaque cycle.
    /// Tente de consommer les ressources nécessaires, puis ajoute la ressource produite.
    /// </summary>
    protected virtual void Produce()
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogError("❌ ResourceManager.Instance is null !");
            return;
        }

        if (inputResourceType != null)
        {
            if (!ResourceManager.Instance.ConsumeResource(inputResourceType.Value, inputAmount))
            {
                Debug.LogWarning($"[Production] Not enough {inputResourceType} to produce {outputResourceType}");
                return;
            }
        }

        ResourceManager.Instance.AddResource(outputResourceType, productionAmount);
    }
}
