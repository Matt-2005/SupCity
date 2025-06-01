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
        outputResourceType = ResourceType.Wood; // Type de ressource produite
        productionAmount = 10;                  // Quantité produite par cycle
        productionInterval = 3f;                // Intervalle de production en secondes
        base.Start();                           // Appel de la logique de base
    }
}
