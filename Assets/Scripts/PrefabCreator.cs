#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Permet de créer un prefab depuis un GameObject dans la scène.
/// À utiliser uniquement dans l’éditeur Unity. Accessible via clic droit (context menu).
/// </summary>
public class PrefabCreator : MonoBehaviour
{
    [Header("Paramètres")]

    /// <summary>
    /// Référence vers l’objet PNJ dans la scène à convertir en prefab.
    /// </summary>
    public GameObject sourcePNJ;

    /// <summary>
    /// Nom du fichier prefab à créer.
    /// </summary>
    public string prefabName = "PNJ";

    /// <summary>
    /// Chemin du dossier dans lequel enregistrer le prefab (relatif au dossier Assets).
    /// </summary>
    public string prefabFolder = "Assets/Prefabs/";

    /// <summary>
    /// Crée un prefab à partir de <see cref="sourcePNJ"/> et l’enregistre à l’emplacement spécifié.
    /// Peut être exécuté depuis le menu contextuel de l’inspecteur.
    /// </summary>
    [ContextMenu("Créer Prefab PNJ depuis l'objet")]
    public void CreerPrefabPNJ()
    {
        if (sourcePNJ == null)
        {
            Debug.LogError("Aucun objet source assigné.");
            return;
        }

        // Crée le dossier si nécessaire
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
