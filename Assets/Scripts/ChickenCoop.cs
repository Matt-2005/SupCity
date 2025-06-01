/// <summary>
/// Bâtiment de production spécialisé dans l’élevage de poules.
/// Produit des œufs à intervalle régulier sans ressource d’entrée.
/// Hérite de <see cref="ProductionBuilding"/>.
/// </summary>
public class ChickenCoop : ProductionBuilding
{
    /// <summary>
    /// Initialise les paramètres de production du poulailler :
    /// type de ressource produite, quantité produite, et intervalle de production.
    /// Appelle ensuite <see cref="ProductionBuilding.Start"/> pour démarrer la production.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.Egg;
        productionAmount = 3;
        productionInterval = 6f;
        base.Start();
    }
}
