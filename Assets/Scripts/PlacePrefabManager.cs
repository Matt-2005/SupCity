using UnityEngine;
using UnityEngine.EventSystems;

public class PlacePrefabManager : MonoBehaviour
{
    public Grid grid;
    private GameObject previewObject = null;

    // Update is called once per frame
    void Update()
{
    // Empêche de placer si souris sur UI
    if (EventSystem.current.IsPointerOverGameObject()) return;

    GameObject selectedPrefab = BuildManager.instance.GetSelectedPrefab();

    if (selectedPrefab != null)
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f; // s’assurer qu’on reste bien en 2D
        Vector3Int cellPos = grid.WorldToCell(mouseWorld);
        Vector3 placePos = grid.CellToWorld(cellPos);

        // S’il n’y a pas encore de preview → on la crée
        if (previewObject == null)
        {
            previewObject = Instantiate(selectedPrefab, placePos, Quaternion.identity);
            SetPreviewMode(previewObject); // on rend l'objet visuellement différent
        }
        else
        {
            // Sinon on le déplace simplement
            previewObject.transform.position = placePos;
        }

        // Clic gauche → place le vrai objet
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(selectedPrefab, placePos, Quaternion.identity);
        }
    }
    else
    {
        // Aucun prefab sélectionné → on supprime le preview s’il existe
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }
}
void SetPreviewMode(GameObject obj)
{
    foreach (var r in obj.GetComponentsInChildren<SpriteRenderer>())
    {
        Color c = r.color;
        c.a = 0.5f; // 50% de transparence
        r.color = c;
    }

    // On désactive les collisions si besoin :
    foreach (var col in obj.GetComponentsInChildren<Collider2D>())
    {
        col.enabled = false;
    }
}

}
