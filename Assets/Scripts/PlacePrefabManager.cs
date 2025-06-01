using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Gère le placement de bâtiments sur une grille à la souris, avec aperçu temps réel.
/// Supporte les objets multi-plaçables (drag pour placer en continu) via le composant <see cref="MultiPlaceable"/>.
/// </summary>
public class PlacePrefabManager : MonoBehaviour
{
    /// <summary>
    /// Référence à la grille Unity utilisée pour le placement.
    /// </summary>
    public Grid grid;

    private GameObject previewObject = null;
    private GameObject lastSelectedPrefab = null;
    private Vector3Int lastPlacedPosition;

    private EmploiManager emploiManager;

    /// <summary>
    /// Initialise les références (notamment à <see cref="EmploiManager"/>).
    /// </summary>
    void Start()
    {
        emploiManager = FindObjectOfType<EmploiManager>();
    }

    /// <summary>
    /// Met à jour l'aperçu de placement et instancie les objets sélectionnés par le joueur sur clic souris.
    /// </summary>
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        GameObject selectedPrefab = BuildManager.instance.GetSelectedPrefab();
        if (selectedPrefab != null)
        {
            bool isMultiPlaceable = selectedPrefab.GetComponent<MultiPlaceable>() != null;

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            Vector3Int cellPos = grid.WorldToCell(mouseWorld);
            Vector3 placePos = grid.CellToWorld(cellPos);

            if (selectedPrefab != lastSelectedPrefab)
            {
                if (previewObject != null) Destroy(previewObject);

                previewObject = Instantiate(selectedPrefab, placePos, Quaternion.identity);
                lastSelectedPrefab = selectedPrefab;
            }
            else if (previewObject != null)
            {
                previewObject.transform.position = placePos;
            }

            if ((Input.GetMouseButtonDown(0) || (isMultiPlaceable && Input.GetMouseButton(0))) && lastPlacedPosition != cellPos)
            {
                GameObject instance = Instantiate(selectedPrefab, placePos, Quaternion.identity);

                emploiManager?.RafraichirListePostes();

                lastPlacedPosition = cellPos;

                AstarPath.active.UpdateGraphs(new Bounds(placePos, Vector3.one));
            }
        }
        else
        {
            if (previewObject != null)
            {
                Destroy(previewObject);
                previewObject = null;
            }

            lastSelectedPrefab = null;
        }
    }
}
