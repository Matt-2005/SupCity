using UnityEngine;

public class SheepFarm : ProductionBuilding
{
    protected override void Start()
    {
        outputResourceType = ResourceType.Wool;
        productionAmount = 2;
        productionInterval = 7f;
        base.Start();
    }
}