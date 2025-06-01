using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Rend un objet (ex: bouton de construction) visible et interactif
/// uniquement lorsque certaines conditions de population et de ressources sont remplies.
/// </summary>
public class UnlockableItem : MonoBehaviour
{
    /// <summary>
    /// Population requise pour débloquer l'élément.
    /// </summary>
    public int requiredPopulation = 0;

    /// <summary>
    /// Type de ressource requis pour le déblocage.
    /// </summary>
    public ResourceType requiredResource = ResourceType.Wood;

    /// <summary>
    /// Quantité de ressource nécessaire.
    /// </summary>
    public int requiredAmount = 0;

    /// <summary>
    /// Cache l'objet au démarrage et vérifie régulièrement s’il peut être débloqué.
    /// </summary>
    void Start()
    {
        gameObject.SetActive(false); // Masquer au départ
        InvokeRepeating(nameof(CheckUnlock), 0.2f, 0.5f);
    }

    /// <summary>
    /// Vérifie si les conditions de déblocage sont remplies
    /// (population et ressource suffisantes).
    /// Si oui, rend l’objet visible et arrête les vérifications.
    /// </summary>
    void CheckUnlock()
    {
        int currentPop = PopulationManager.Instance?.GetPopulation() ?? 0;
        int currentAmount = ResourceManager.Instance?.GetResource(requiredResource) ?? 0;

        if (currentPop >= requiredPopulation && currentAmount >= requiredAmount)
        {
            gameObject.SetActive(true);
            CancelInvoke(nameof(CheckUnlock));
        }
    }
}
