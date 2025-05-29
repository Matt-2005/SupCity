using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Gère l’aperçu et le placement des prefabs sélectionnés dans la grille.
/// Affiche un objet fantôme (preview) sous la souris et instancie le prefab au clic.
/// Prend en charge le placement en continu si le prefab est marqué comme <c>MultiPlaceable</c>.
/// </summary>
public class PlacePrefabManager : MonoBehaviour
{
    /// <summary>Grille de placement utilisée pour aligner les objets sur la carte.</summary>
    public Grid grid;

    /// <summary>Objet fantôme affiché sous la souris pour prévisualiser le placement.</summary>
    private GameObject previewObject = null;

    /// <summary>Dernier prefab sélectionné pour placement (évite les instanciations inutiles).</summary>
    private GameObject lastSelectedPrefab = null;

    /// <summary>Dernière position de cellule où un objet a été placé (évite les doublons).</summary>
    private Vector3Int lastPlacedPosition;

    /// <summary>
    /// Met à jour l’aperçu et place le prefab si l’utilisateur clique dans la grille.
    /// Ignore les clics sur l’interface UI grâce à <c>EventSystem</c>.
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
                if (previewObject != null)
                {
                    Destroy(previewObject);
                }
                previewObject = Instantiate(selectedPrefab, placePos, Quaternion.identity);
                lastSelectedPrefab = selectedPrefab;
            }
            else
            {
                if (previewObject != null)
                {
                    previewObject.transform.position = placePos;
                }
            }

            if (Input.GetMouseButtonDown(0) || (isMultiPlaceable && Input.GetMouseButton(0)))
            {
                if (lastPlacedPosition != cellPos)
                {
                    Instantiate(selectedPrefab, placePos, Quaternion.identity);
                    lastPlacedPosition = cellPos;

                    // Met à jour la grille de navigation (A* Pathfinding)
                    AstarPath.active.UpdateGraphs(new Bounds(placePos, Vector3.one));
                }
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
