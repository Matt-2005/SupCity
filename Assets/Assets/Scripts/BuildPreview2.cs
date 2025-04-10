using UnityEngine;

public class BuildPreview2 : MonoBehaviour
{
    public GameObject previewPrefab; // Le prefab fantôme
    private GameObject currentPreview;

    public Grid grid; // Assure-toi de référencer ta Grid

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = grid.WorldToCell(mouseWorldPos);
        Vector3 snappedPos = grid.CellToWorld(cellPos);

        if (currentPreview == null)
        {
            currentPreview = Instantiate(previewPrefab);
        }

        currentPreview.transform.position = snappedPos;

        // Valider le placement
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding(snappedPos);
        }
    }

    void PlaceBuilding(Vector3 pos)
    {
        // Remplace par ton prefab réel à placer
        GameObject buildingToPlace = BuildManager.instance.GetSelectedPrefab();

        Instantiate(buildingToPlace, pos, Quaternion.identity);
        Destroy(currentPreview);
        BuildManager.instance.ClearSelectedPrefab(); // facultatif
        this.enabled = false; // désactive la preview
    }
}
