using UnityEngine;

/// <summary>
/// Usine qui transforme le bois en outils en bois.
/// Consomme 3 bois et produit 2 outils bois toutes les 15 secondes.
/// </summary>
public class WoodToolFactory : ProductionBuilding
{
    // Type de ressource requise pour la production
    protected override ResourceType? inputResourceType => ResourceType.Wood;

    // Quantité de ressource requise
    protected override int inputAmount => 3;

    /// <summary>
    /// Initialise les paramètres de production pour l'usine d'outils bois.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.WoodTools;
        productionAmount = 2;
        productionInterval = 15f;
        base.Start();
    }
}
