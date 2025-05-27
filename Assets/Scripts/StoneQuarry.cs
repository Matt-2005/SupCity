using UnityEngine;
public class StoneQuarry : ProductionBuilding
{
    protected override void Start()
    {
        outputResourceType = ResourceType.Stone;
        productionAmount = 10;
        productionInterval = 3f;
        base.Start();
    }
}