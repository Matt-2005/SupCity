using TMPro;
using UnityEngine;

/// <summary>
/// Gère la sélection d’un prefab à construire et la suppression d’objets dans la scène.
/// Permet de basculer entre les modes de construction et d'effacement.
/// Utilise le singleton BuildManager.instance pour un accès global.
/// </summary>
public class BuildManager : MonoBehaviour
{
    /// <summary>
    /// Instance unique du BuildManager (singleton).
    /// </summary>
    public static BuildManager instance;

    /// <summary>
    /// Prefab actuellement sélectionné pour la construction.
    /// </summary>
    private GameObject selectedPrefab;

    /// <summary>
    /// Index du bouton actuellement utilisé (si utile pour l’UI).
    /// </summary>
    private int currentButton;

    /// <summary>
    /// Indique si le mode suppression est activé.
    /// </summary>
    private bool isErasing = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

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
    /// Définit le prefab sélectionné pour la construction.
    /// </summary>
    public void SelectPrefab(GameObject prefab) {
        selectedPrefab = prefab;
    }

    /// <summary>
    /// Retourne le prefab actuellement sélectionné.
    /// </summary>
    public GameObject GetSelectedPrefab() {
        return selectedPrefab;
    }

    /// <summary>
    /// Réinitialise le prefab sélectionné.
    /// </summary>
    public void ClearSelectedPrefab() {
        selectedPrefab = null;
    }

    /// <summary>
    /// Active le mode suppression d’objet.
    /// </summary>
    public void EraseObject() {
        ClearSelectedPrefab();
        isErasing = true;
    }
}
