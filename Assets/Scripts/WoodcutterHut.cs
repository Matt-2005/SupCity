using UnityEngine;

/// <summary>
/// Bâtiment de production de bois. Génère du bois à intervalles réguliers.
/// </summary>
public class WoodcutterHut : ProductionBuilding
{
    /// <summary>
    /// Initialise les paramètres de production pour la cabane de bûcheron.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.Wood;
        productionAmount = 10;
        productionInterval = 3f;
        base.Start();
    }
}
