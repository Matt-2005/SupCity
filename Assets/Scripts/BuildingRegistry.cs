using System.Collections.Generic;
using UnityEngine;

public class BuildingRegistry : MonoBehaviour
{
    public static BuildingRegistry Instance;

    private Dictionary<string, List<GameObject>> batimentsParTag = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RegisterBuilding(GameObject building)
    {
        string tag = building.tag;

        if (!batimentsParTag.ContainsKey(tag))
        {
            batimentsParTag[tag] = new List<GameObject>();
        }

        if (!batimentsParTag[tag].Contains(building))
        {
            batimentsParTag[tag].Add(building);
        }
    }

    public void UnregisterBuilding(GameObject building)
    {
        string tag = building.tag;

        if (batimentsParTag.ContainsKey(tag))
        {
            batimentsParTag[tag].Remove(building);
        }
    }

    public List<GameObject> GetBuildingsWithTag(string tag)
    {
        if (batimentsParTag.ContainsKey(tag))
        {
            return batimentsParTag[tag];
        }

        return new List<GameObject>();
    }
}
