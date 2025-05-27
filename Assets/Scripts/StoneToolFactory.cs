using UnityEngine;
public class StoneToolFactory : ProductionBuilding
{
    protected override ResourceType? inputResourceType => ResourceType.Stone;
    protected override int inputAmount => 3;

    protected override void Start()
    {
        outputResourceType = ResourceType.StoneTools;
        productionAmount = 2;
        productionInterval = 15;
        base.Start();
    }
}