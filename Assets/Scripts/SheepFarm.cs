using UnityEngine;

/// <summary>
/// Représente une ferme de moutons qui produit de la laine.
/// Hérite de <see cref="ProductionBuilding"/> pour gérer la production automatique.
/// </summary>
public class SheepFarm : ProductionBuilding
{
    /// <summary>
    /// Initialise les paramètres spécifiques à la production de laine.
    /// </summary>
    protected override void Start()
    {
        outputResourceType = ResourceType.Wool;  // Type de ressource produite : Laine
        productionAmount = 2;                    // Quantité produite à chaque intervalle
        productionInterval = 7f;                 // Temps entre chaque production (en secondes)
        base.Start();                            // Appelle le Start() de ProductionBuilding
    }
}
