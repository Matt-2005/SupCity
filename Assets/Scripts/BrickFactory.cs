using UnityEngine;
public class BrickFactory : ProductionBuilding
{
    protected override ResourceType? inputResourceType => ResourceType.Clay;
    protected override int inputAmount => 4;

    protected override void Start()
    {
        outputResourceType = ResourceType.Brick;
        productionAmount = 3;
        productionInterval = 10f;
        base.Start();
    }
}