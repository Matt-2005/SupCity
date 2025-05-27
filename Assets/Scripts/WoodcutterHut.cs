using UnityEngine;
public class WoodcutterHut : ProductionBuilding
{
    protected override void Start()
    {
        outputResourceType = ResourceType.Wood;
        productionAmount = 10;
        productionInterval = 3f;
        base.Start();
    }
}