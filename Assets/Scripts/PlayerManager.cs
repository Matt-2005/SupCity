using UnityEngine;

/// <summary>
/// Gère le comportement des PNJ en basculant du déplacement aléatoire vers un système de besoins avec pathfinding.
/// Active automatiquement les composants <c>BesoinPlayers</c> et <c>PathfindingAI</c> lorsqu’un feu est détecté.
/// </summary>
public class PlayerBehaviourManager : MonoBehaviour
{
    private RandomNPC randomNPC;
    private BesoinPlayers besoinPlayers;
    private PathfindingAI pathfindingAI;

    void Start()
    {
        randomNPC = GetComponentInChildren<RandomNPC>();
        besoinPlayers = GetComponent<BesoinPlayers>();
        pathfindingAI = GetComponent<PathfindingAI>();
    }

    private bool modeActif = false;
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

    void ActiverModePathfinding()
    {
        /*if (randomNPC != null)
        {
            randomNPC.transform.position = transform.position;
            randomNPC.enabled = false;
        }*/

        if (besoinPlayers != null) besoinPlayers.enabled = true;
        if (pathfindingAI != null) pathfindingAI.enabled = true;
    }
}
