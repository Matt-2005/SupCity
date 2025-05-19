using UnityEngine;

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
        // Ne vérifie que si ce n'est pas encore activé
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
    if (randomNPC != null) randomNPC.enabled = false;
    if (besoinPlayers != null) besoinPlayers.enabled = true;
    if (pathfindingAI != null) pathfindingAI.enabled = true;
}


}
