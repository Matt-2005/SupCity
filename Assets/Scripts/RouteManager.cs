using UnityEngine;
using System.Collections;

public class RouteManager : MonoBehaviour
{
    public static RouteManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

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

    private IEnumerator AttenteEtSuite(GameObject pnj, Transform destinationFinale)
    {
        yield return new WaitForSeconds(1.5f);

        if (pnj != null && destinationFinale != null)
        {
            pnj.GetComponent<PathfindingAI>().setTarget(destinationFinale);
        }
    }
}
