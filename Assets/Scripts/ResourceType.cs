using UnityEngine;

/// <summary>
/// Enumération représentant les différents types de ressources utilisés dans le jeu.
/// </summary>
public enum ResourceType
{
    /// <summary>Bois, utilisé pour la construction ou les outils.</summary>
    Wood,

    /// <summary>Pierre, ressource de base pour les constructions solides.</summary>
    Stone,

    /// <summary>Argile, utilisée notamment pour fabriquer des briques.</summary>
    Clay,

    /// <summary>Nourriture générique (peut être remplacée par des types spécifiques comme Berry, Egg...)</summary>
    Food,

    /// <summary>Baies, ressource alimentaire récoltée dans les champs de baies.</summary>
    Berry,

    /// <summary>Laine, produite par les moutons, utilisée pour les vêtements ou le commerce.</summary>
    Wool,

    /// <summary>Œufs, ressource alimentaire produite par les poules.</summary>
    Egg,

    /// <summary>Eau, ressource vitale utilisée pour les besoins des personnages.</summary>
    Water,

    /// <summary>Outils en bois, utilisés dans certaines productions ou métiers.</summary>
    WoodTools,

    /// <summary>Outils en pierre, équivalents améliorés pour certaines productions.</summary>
    StoneTools,

    /// <summary>Briques, matériau de construction avancé fabriqué à partir d’argile.</summary>
    Brick
}
