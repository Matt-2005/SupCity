public class ChickenCoop : ProductionBuilding
{
    protected override void Start()
    {
        outputResourceType = ResourceType.Egg;
        productionAmount = 3;
        productionInterval = 6f;
        base.Start();
    }
}
