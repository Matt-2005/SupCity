using UnityEngine;
public class WoodcutterHut : ProductionBuilding
{
    protected override void Start()
    {
        outputResourceType = ResourceType.Wood;
        productionAmount = 5;
        productionInterval = 5f;
        base.Start();
    }
}