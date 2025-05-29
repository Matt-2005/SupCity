using UnityEngine;
using UnityEngine.EventSystems;


public class PlacePrefabManager : MonoBehaviour
{
    public Grid grid;
    private GameObject previewObject = null;
    private GameObject lastSelectedPrefab = null;
    private Vector3Int lastPlacedPosition;
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
