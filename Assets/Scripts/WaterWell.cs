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
        outputResourceType = ResourceType.Water; // Type de ressource produite
        productionAmount = 5;                    // Quantité produite par cycle
        productionInterval = 5f;                 // Intervalle de production en secondes
        base.Start();                            // Appelle le comportement de base de ProductionBuilding
    }
}
