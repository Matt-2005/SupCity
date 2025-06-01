using UnityEngine;

/// <summary>
/// Bâtiment de production d'eau. Produit automatiquement une quantité définie à intervalle régulier.
/// </summary>
public class WaterWell : ProductionBuilding
{
    /// <summary>
    /// Initialise les paramètres de production pour le puits.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.Water;
        productionAmount = 5;
        productionInterval = 5f;
        base.Start();
    }
}
