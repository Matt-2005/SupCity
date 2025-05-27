using UnityEngine;

public abstract class ProductionBuilding : MonoBehaviour
{
    [Header("Production Settings")]
    public ResourceType outputResourceType;
    public int productionAmount = 1;
    public float productionInterval = 5f;

    protected virtual ResourceType? inputResourceType => null;
    protected virtual int inputAmount => 0;

    private float timer;

    protected virtual void Start()
    {
        timer = productionInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Produce();
            timer = productionInterval;
        }
    }

    protected virtual void Produce()
    {
        if (inputResourceType != null)
        {
            if (!ResourceManager.Instance.ConsumeResource(inputResourceType.Value, inputAmount))
            {
                Debug.LogWarning($"[Production] Not enough {inputResourceType} to produce {outputResourceType}");
                return;
            }
        }

        ResourceManager.Instance.AddResource(outputResourceType, productionAmount);
    }
}