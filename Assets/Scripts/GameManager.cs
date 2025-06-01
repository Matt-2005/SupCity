using UnityEngine;

/// <summary>
/// Gère les éléments globaux du jeu.
/// Ce script est destiné à être étendu pour centraliser la logique de gestion globale (ex. : pause, sauvegarde, chargement, état du jeu).
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Appelé dès le chargement du GameObject dans la scène.
    /// Utilisé ici pour signaler l'initialisation du GameManager.
    /// </summary>
    void Awake()
    {
        Debug.Log("🎮 GameManager initialisé");
    }
}
