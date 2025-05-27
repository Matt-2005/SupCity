using UnityEngine;
public class ClayHut : ProductionBuilding
{
    protected override void Start()
    {
        outputResourceType = ResourceType.Clay;
        productionAmount = 4;
        productionInterval = 6f;
        base.Start();
    }
}
