using UnityEngine;
using UnityEngine.UI;

public class UnlockableItem : MonoBehaviour
{
    public int requiredPopulation = 0;
    public ResourceType requiredResource = ResourceType.Wood;
    public int requiredAmount = 0;

    void Start()
    {
        gameObject.SetActive(false); // Rendre le bouton invisible au début
        InvokeRepeating(nameof(CheckUnlock), 0.2f, 0.5f);
    }

    void CheckUnlock()
    {
        int currentPop = PopulationManager.Instance?.GetPopulation() ?? 0;
        int currentAmount = ResourceManager.Instance?.GetResource(requiredResource) ?? 0;

        if (currentPop >= requiredPopulation && currentAmount >= requiredAmount)
        {
            gameObject.SetActive(true); // Rendre visible et interactif
            CancelInvoke(nameof(CheckUnlock));
        }
    }
}
