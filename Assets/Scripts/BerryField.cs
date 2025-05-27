using UnityEngine;

public class BerryField : ProductionBuilding
{
    protected override void Start()
    {
        outputResourceType = ResourceType.Berry;
        productionAmount = 10;
        productionInterval = 4f;
        base.Start();
    }
}