using TMPro;
using UnityEngine;

/// <summary>
/// Gère l'affichage en temps réel des ressources dans l'interface utilisateur (UI).
/// Met à jour les textes selon les valeurs stockées dans le <see cref="ResourceManager"/>.
/// </summary>
public class ResourceDisplayUI : MonoBehaviour
{
    [Header("Références UI pour chaque ressource")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI clayText;
    public TextMeshProUGUI brickText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI woodToolsText;
    public TextMeshProUGUI stoneToolsText;

    /// <summary>
    /// Met à jour les valeurs de ressources à chaque frame.
    /// </summary>
    void Update()
    {
        if (ResourceManager.Instance == null) return;

        woodText.text = "Bois : " + ResourceManager.Instance.GetResource(ResourceType.Wood);
        stoneText.text = "Pierre : " + ResourceManager.Instance.GetResource(ResourceType.Stone);
        clayText.text = "Argile : " + ResourceManager.Instance.GetResource(ResourceType.Clay);
        brickText.text = "Briques : " + ResourceManager.Instance.GetResource(ResourceType.Brick);
        waterText.text = "Eau : " + ResourceManager.Instance.GetResource(ResourceType.Water);
        woodToolsText.text = "Outils bois : " + ResourceManager.Instance.GetResource(ResourceType.WoodTools);
        stoneToolsText.text = "Outils pierre : " + ResourceManager.Instance.GetResource(ResourceType.StoneTools);
    }
}
