using TMPro;
using UnityEngine;

/// <summary>
/// Gère la sélection et le placement d’objets dans le jeu, ainsi que leur suppression.
/// Utilise un pattern Singleton accessible via <see cref="instance"/>.
/// </summary>
public class BuildManager : MonoBehaviour
{
    /// <summary>
    /// Instance unique du BuildManager (Singleton).
    /// </summary>
    public static BuildManager instance;

    /// <summary>
    /// Préfab sélectionné pour l’instanciation.
    /// </summary>
    private GameObject selectedPrefab;

    /// <summary>
    /// Index du bouton actuellement sélectionné (non utilisé dans ce script).
    /// </summary>
    private int currentButton;

    /// <summary>
    /// Indique si le mode suppression d’objet est actif.
    /// </summary>
    private bool isErasing = false;

    /// <summary>
    /// Initialise l’instance Singleton. Détruit les doublons éventuels.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Si le mode suppression est activé, détecte les clics sur des objets supprimables et les détruit.
    /// </summary>
    private void Update()
    {
        if (isErasing)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

                if (hit != null && hit.GetComponent<ObjectEraseable>())
                {
                    Destroy(hit.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Définit le prefab actuellement sélectionné pour l’instanciation.
    /// </summary>
    /// <param name="prefab">Le prefab à placer.</param>
    public void SelectPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
    }

    /// <summary>
    /// Récupère le prefab actuellement sélectionné.
    /// </summary>
    /// <returns>Le prefab sélectionné ou null.</returns>
    public GameObject GetSelectedPrefab()
    {
        return selectedPrefab;
    }

    /// <summary>
    /// Désélectionne le prefab actuellement sélectionné.
    /// </summary>
    public void ClearSelectedPrefab()
    {
        selectedPrefab = null;
    }

    /// <summary>
    /// Active le mode suppression d’objet et désélectionne tout prefab.
    /// </summary>
    public void EraseObject()
    {
        ClearSelectedPrefab();
        isErasing = true;
    }
}
