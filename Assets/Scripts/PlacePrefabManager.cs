using UnityEngine;
using UnityEngine.EventSystems;

public class PlacePrefabManager : MonoBehaviour
{
    public Grid grid;

    // Update is called once per frame
    void Update()
    {
        // Empêche de placer si tu cliques sur un bouton
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject prefab = BuildManager.instance.GetSelectedPrefab();
            if (prefab != null)
            {
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = grid.WorldToCell(mouseWorld);
                Vector3 placePos = grid.CellToWorld(cellPos);

                Instantiate(prefab, placePos, Quaternion.identity);
            }
        }
    }
}
