using UnityEngine;
using System.Collections;

/// <summary>
/// Gère les routes dans la scène et permet aux PNJ d'y transiter pour atteindre une destination.
/// Utilise le pattern Singleton pour être facilement accessible depuis d'autres scripts.
/// </summary>
public class RouteManager : MonoBehaviour
{
    /// <summary>
    /// Instance unique (Singleton) de ce manager.
    /// </summary>
    public static RouteManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Trouve la route la plus proche d’une position donnée.
    /// </summary>
    /// <param name="position">Position depuis laquelle chercher la route la plus proche.</param>
    /// <returns>La GameObject de la route la plus proche, ou null si aucune route n’est trouvée.</returns>
    public GameObject GetRouteLaPlusProche(Vector3 position)
    {
        GameObject[] routes = GameObject.FindGameObjectsWithTag("Route");
        if (routes.Length == 0) return null;

        GameObject plusProche = null;
        float distanceMin = Mathf.Infinity;

        foreach (GameObject route in routes)
        {
            float distance = Vector3.Distance(position, route.transform.position);
            if (distance < distanceMin)
            {
                distanceMin = distance;
                plusProche = route;
            }
        }

        return plusProche;
    }

    /// <summary>
    /// Fait passer un PNJ par la route la plus proche avant d’aller à sa destination finale.
    /// Si aucune route n’est trouvée, le PNJ se rend directement à sa destination.
    /// </summary>
    /// <param name="pnj">Le GameObject du PNJ.</param>
    /// <param name="destinationFinale">La destination finale à atteindre après la route.</param>
    public void AllerViaRoute(GameObject pnj, Transform destinationFinale)
    {
        GameObject route = GetRouteLaPlusProche(pnj.transform.position);

        if (route != null)
        {
            pnj.GetComponent<PathfindingAI>().setTarget(route.transform);
            pnj.GetComponent<BesoinPlayers>().StartCoroutine(AttenteEtSuite(pnj, destinationFinale));
        }
        else
        {
            // Aucun chemin → aller direct
            pnj.GetComponent<PathfindingAI>().setTarget(destinationFinale);
        }
    }

    /// <summary>
    /// Coroutine qui attend un court délai avant d’envoyer le PNJ vers sa destination finale.
    /// Donne le temps au PNJ d’arriver à la route avant de continuer son trajet.
    /// </summary>
    private IEnumerator AttenteEtSuite(GameObject pnj, Transform destinationFinale)
    {
        yield return new WaitForSeconds(1.5f);

        if (pnj != null && destinationFinale != null)
        {
            pnj.GetComponent<PathfindingAI>().setTarget(destinationFinale);
        }
    }
}
