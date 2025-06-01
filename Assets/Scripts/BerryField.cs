using UnityEngine;

/// <summary>
/// Bâtiment de production spécialisé dans la récolte de baies.
/// Hérite de <see cref="ProductionBuilding"/> et configure les paramètres de production.
/// </summary>
public class BerryField : ProductionBuilding
{
    /// <summary>
    /// Initialise les paramètres spécifiques au champ de baies :
    /// type de ressource produite, quantité produite, et intervalle de production.
    /// Appelle ensuite la méthode <see cref="ProductionBuilding.Start"/> pour initialiser la production.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.Berry;
        productionAmount = 10;
        productionInterval = 4f;
        base.Start();
    }
}
