using TMPro;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    public static BuildManager instance;
    private GameObject selectedPrefab;
    private int currentButton;
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
            if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

            if (hit != null && hit.CompareTag("Object")) // ou autre condition
            {
                Destroy(hit.gameObject);
            }
        }
        }
        
    }

    
    public void SelectPrefab(GameObject prefab) {
        selectedPrefab = prefab;
    }

    public GameObject GetSelectedPrefab() {
        return selectedPrefab;
    }

    public void ClearSelectedPrefab() {
        selectedPrefab = null;
    }

    public void EraseObject() {
        ClearSelectedPrefab();
        isErasing = true;
    }
}
