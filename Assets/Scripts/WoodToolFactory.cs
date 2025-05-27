using UnityEngine;
public class WoodToolFactory : ProductionBuilding
{
    protected override ResourceType? inputResourceType => ResourceType.Wood;
    protected override int inputAmount => 3;

    protected override void Start()
    {
        outputResourceType = ResourceType.WoodTools;
        productionAmount = 2;
        productionInterval = 15f;
        base.Start();
    }
}