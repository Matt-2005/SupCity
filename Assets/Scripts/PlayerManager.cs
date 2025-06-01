using UnityEngine;

/// <summary>
/// Gère le comportement des PNJ en basculant du déplacement aléatoire vers un système de besoins avec pathfinding.
/// Active automatiquement les composants <c>BesoinPlayers</c> et <c>PathfindingAI</c> lorsqu’un objet avec le tag "feu" est détecté dans la scène.
/// </summary>
public class PlayerBehaviourManager : MonoBehaviour
{
    private RandomNPC randomNPC;
    private BesoinPlayers besoinPlayers;
    private PathfindingAI pathfindingAI;

    /// <summary>
    /// Indique si le mode pathfinding est déjà activé pour éviter une activation multiple.
    /// </summary>
    private bool modeActif = false;

    /// <summary>
    /// Initialise les références aux composants nécessaires sur le PNJ.
    /// </summary>
    void Start()
    {
        randomNPC = GetComponentInChildren<RandomNPC>();
        besoinPlayers = GetComponent<BesoinPlayers>();
        pathfindingAI = GetComponent<PathfindingAI>();
    }

    /// <summary>
    /// Surveille l'apparition d'un objet avec le tag "feu" et déclenche le passage au mode pathfinding.
    /// </summary>
    void Update()
    {
        if (!modeActif)
        {
            GameObject feuObj = GameObject.FindWithTag("feu");
            if (feuObj != null && feuObj.activeInHierarchy)
            {
                Debug.Log("🔥 Feu détecté, activation du mode pathfinding");
                ActiverModePathfinding();
                modeActif = true;
            }
        }
    }

    /// <summary>
    /// Active le mode besoins + pathfinding en désactivant le déplacement aléatoire.
    /// Réinitialise les jauges de besoins pour forcer une action immédiate.
    /// </summary>
    void ActiverModePathfinding()
    {
        if (randomNPC != null)
        {
            randomNPC.transform.position = transform.position;
            randomNPC.enabled = false;
        }

        if (besoinPlayers != null)
        {
            besoinPlayers.enabled = true;
            besoinPlayers.faim = 0f; 
            besoinPlayers.soif = 0f;
            besoinPlayers.energie = 0f;

            besoinPlayers.SendMessage("Start");
        }

        if (pathfindingAI != null)
        {
            pathfindingAI.enabled = true;
        }
    }
}
