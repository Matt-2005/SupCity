using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
        Debug.Log($"[ResourceManager] +{amount} {type} (Total: {resources[type]})");
    }

    public bool ConsumeResource(ResourceType type, int amount)
    {
        if (resources.ContainsKey(type) && resources[type] >= amount)
        {
            resources[type] -= amount;
            Debug.Log($"[ResourceManager] -{amount} {type} (Remaining: {resources[type]})");
            return true;
        }
        return false;
    }

    public int GetResource(ResourceType type)
    {
        return resources[type];
    }
}