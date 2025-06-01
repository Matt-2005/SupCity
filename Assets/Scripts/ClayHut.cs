using UnityEngine;

/// <summary>
/// Bâtiment de production d’argile.
/// Génère automatiquement de l’argile à intervalle régulier.
/// Hérite de <see cref="ProductionBuilding"/>.
/// </summary>
public class ClayHut : ProductionBuilding
{
    /// <summary>
    /// Initialise les paramètres de production de la hutte d’argile :
    /// type de ressource produite, quantité produite, et intervalle de production.
    /// Appelle ensuite <see cref="ProductionBuilding.Start"/> pour démarrer la production.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.Clay;
        productionAmount = 4;
        productionInterval = 6f;
        base.Start();
    }
}
