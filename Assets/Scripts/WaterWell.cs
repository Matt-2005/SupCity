using UnityEngine;
public class WaterWell : ProductionBuilding
{
    protected override void Start()
    {
        outputResourceType = ResourceType.Water;
        productionAmount = 5;
        productionInterval = 5f;
        base.Start();
    }
}