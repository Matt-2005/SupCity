using UnityEngine;

/// <summary>
/// Usine de production de briques.  
/// Consomme de l'argile pour produire des briques à intervalles réguliers.
/// Hérite de <see cref="ProductionBuilding"/>.
/// </summary>
public class BrickFactory : ProductionBuilding
{
    /// <summary>
    /// Ressource requise en entrée pour produire des briques : argile.
    /// </summary>
    protected override ResourceType? inputResourceType => ResourceType.Clay;

    /// <summary>
    /// Quantité d'argile nécessaire pour produire les briques.
    /// </summary>
    protected override int inputAmount => 4;

    /// <summary>
    /// Initialise la production de briques avec les paramètres définis :
    /// type de ressource produite, quantité produite, intervalle de production.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.Brick;
        productionAmount = 3;
        productionInterval = 10f;
        base.Start();
    }
}
