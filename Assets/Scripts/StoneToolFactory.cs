using UnityEngine;

/// <summary>
/// Représente une usine de fabrication d'outils en pierre.
/// Nécessite de la pierre comme ressource d'entrée et produit des outils en pierre.
/// Hérite de la classe <c>ProductionBuilding</c>.
/// </summary>
public class StoneToolFactory : ProductionBuilding
{
    /// <summary>
    /// Type de ressource requis pour produire : pierre.
    /// </summary>
    protected override ResourceType? inputResourceType => ResourceType.Stone;

    /// <summary>
    /// Quantité de pierre nécessaire à chaque cycle de production.
    /// </summary>
    protected override int inputAmount => 3;

    /// <summary>
    /// Initialise l'usine avec les paramètres spécifiques :
    /// produit 2 outils en pierre toutes les 15 secondes.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.StoneTools;
        productionAmount = 2;
        productionInterval = 15f;
        base.Start();
    }
}
