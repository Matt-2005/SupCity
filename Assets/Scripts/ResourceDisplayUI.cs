using TMPro;
using UnityEngine;

public class ResourceDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI clayText;
    public TextMeshProUGUI brickText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI woodToolsText;
    public TextMeshProUGUI stoneToolsText;

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
