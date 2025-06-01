using UnityEngine;

/// <summary>
/// Représente une carrière de pierre dans le jeu.
/// Hérite de la classe <c>ProductionBuilding</c> et produit de la pierre à intervalle régulier.
/// </summary>
public class StoneQuarry : ProductionBuilding
{
    /// <summary>
    /// Initialise la carrière avec les paramètres de production spécifiques :
    /// produit 10 unités de pierre toutes les 3 secondes.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.Stone;
        productionAmount = 10;
        productionInterval = 3f;
        base.Start();
    }
}
