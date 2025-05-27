#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PrefabCreator : MonoBehaviour
{
    [Header("Paramètres")]
    public GameObject sourcePNJ; // L'objet PNJ dans la scène
    public string prefabName = "PNJ";
    public string prefabFolder = "Assets/Prefabs/";

    [ContextMenu("Créer Prefab PNJ depuis l'objet")]
    public void CreerPrefabPNJ()
    {
        if (sourcePNJ == null)
        {
            Debug.LogError("Aucun objet source assigné.");
            return;
        }

        // Crée le dossier s’il n'existe pas
        if (!AssetDatabase.IsValidFolder(prefabFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        string prefabPath = prefabFolder + prefabName + ".prefab";

        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(sourcePNJ, prefabPath, InteractionMode.UserAction);
        if (prefab != null)
        {
            Debug.Log($"✅ Prefab enregistré à : {prefabPath}");
        }
        else
        {
            Debug.LogError("❌ Échec de la création du prefab.");
        }
    }
}
#endif
