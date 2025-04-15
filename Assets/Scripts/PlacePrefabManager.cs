using UnityEngine;
using UnityEngine.EventSystems;

public class PlacePrefabManager : MonoBehaviour
{
    public Grid grid;
    private GameObject previewObject = null;
    private GameObject lastSelectedPrefab = null;


    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        GameObject selectedPrefab = BuildManager.instance.GetSelectedPrefab();

        if (selectedPrefab != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            Vector3Int cellPos = grid.WorldToCell(mouseWorld);
            Vector3 placePos = grid.CellToWorld(cellPos);

            // 🆕 Si le prefab sélectionné a changé, on détruit l’ancien preview
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
                // Sinon on déplace simplement le preview existant
                if (previewObject != null)
                    previewObject.transform.position = placePos;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(selectedPrefab, placePos, Quaternion.identity);
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
