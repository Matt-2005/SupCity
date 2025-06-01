using UnityEngine;

/// <summary>
/// Gère les besoins fondamentaux (faim, soif, énergie) et les comportements associés des PNJ.
/// Permet aux personnages d'agir selon leurs priorités : manger, boire, dormir ou travailler.
/// </summary>
public class BesoinPlayers : MonoBehaviour
{
    /// <summary>Valeur actuelle de la soif (entre 0 et 1).</summary>
    public float soif = 1f;

    /// <summary>Valeur actuelle de la faim (entre 0 et 1).</summary>
    public float faim = 1f;

    /// <summary>Valeur actuelle de l'énergie (entre 0 et 1).</summary>
    public float energie = 1f;

    /// <summary>Taux de diminution de la faim par seconde.</summary>
    public float tauxFaim = 0.02f;

    /// <summary>Taux de diminution de la soif par seconde.</summary>
    public float tauxSoif = 0.01f;

    /// <summary>Taux de diminution de l'énergie par seconde.</summary>
    public float tauxEnergie = 0.01f;

    /// <summary>Types de besoins pouvant être prioritaires pour un PNJ.</summary>
    public enum BesoinType { Rien, Faim, Soif, Energie }

    /// <summary>Seuil à partir duquel un besoin est considéré critique et doit être comblé en priorité.</summary>
    public float seuilCritique = 0.15f;

    /// <summary>Seuil à partir duquel un besoin peut déclencher une action (non utilisé dans ce script).</summary>
    public float seuilAction = 0.3f;

    /// <summary>États possibles du PNJ en fonction de ses actions ou déplacements.</summary>
    public enum EtatPNJ { Idle, AllerManger, Manger, AllerBoire, Boire, AllerDormir, Dormir, AllerTravailler, Travailler }

    /// <summary>État actuel du PNJ.</summary>
    private EtatPNJ etatActuel = EtatPNJ.Idle;

    private bool enAction = false;
    private bool estChezSoi = false;

    /// <summary>Indique si le PNJ possède un logement attribué.</summary>
    public bool aUnLogement = false;

    private BesoinType besoinActuel;
    private float actionTimer = 0f;
    private float actionDuration = 2f;

    /// <summary>Liste des tags d’usines pour la recherche d’un lieu de travail.</summary>
    private string[] tagsUsines = { "usineArgile", "usineBrique", "usineEau", "usineBaie", "usineBois", "usineOutils", "usinePierre", "usineOutilsPierre", "potterie", "enclotMouton", "enclotPoule" };

    /// <summary>
    /// Met à jour les besoins du PNJ et gère les transitions d'état en fonction des priorités.
    /// </summary>
    void Update()
    {
        faim = Mathf.Clamp01(faim - tauxFaim * Time.deltaTime);
        soif = Mathf.Clamp01(soif - tauxSoif * Time.deltaTime);
        energie = Mathf.Clamp01(energie - tauxEnergie * Time.deltaTime);

        Debug.Log($"\u00c9tat actuel : {etatActuel} | Faim: {faim:F2} | Soif: {soif:F2} | \u00c9nergie: {energie:F2}");

        if (etatActuel == EtatPNJ.AllerManger || etatActuel == EtatPNJ.AllerBoire || etatActuel == EtatPNJ.AllerDormir || etatActuel == EtatPNJ.AllerTravailler)
        {
            var currentTarget = GetComponent<PathfindingAI>().target;
            if (currentTarget == null)
            {
                Debug.LogWarning("La ressource a été détruite avant l’arrivée. Retour à Idle.");
                etatActuel = EtatPNJ.Idle;
                return;
            }
        }

        switch (etatActuel)
        {
            case EtatPNJ.Idle:
                besoinActuel = GetBesoinPrioritaire();
                Debug.Log($"Besoin prioritaire détecté : {besoinActuel}");
                switch (besoinActuel)
                {
                    case BesoinType.Faim:
                        Manger();
                        break;
                    case BesoinType.Soif:
                        Boire();
                        break;
                    case BesoinType.Energie:
                        Dormir();
                        break;
                    case BesoinType.Rien:
                        Travailler();
                        break;
                }
                break;

            case EtatPNJ.Manger:
                actionTimer += Time.deltaTime;
                if (actionTimer >= actionDuration)
                {
                    FinManger();
                }
                break;
            case EtatPNJ.Boire:
                actionTimer += Time.deltaTime;
                if (actionTimer >= actionDuration)
                {
                    FinBoire();
                }
                break;
            case EtatPNJ.Dormir:
                actionTimer += Time.deltaTime;
                if (actionTimer >= actionDuration)
                {
                    FinDormir();
                }
                break;
            case EtatPNJ.Travailler:
                actionTimer += Time.deltaTime;
                if (actionTimer >= actionDuration)
                {
                    FinTravailler();
                }
                break;
        }
    }

    /// <summary>
    /// Retourne le besoin prioritaire du PNJ en fonction des seuils critiques.
    /// </summary>
    public BesoinType GetBesoinPrioritaire()
    {
        if (faim <= seuilCritique) return BesoinType.Faim;
        if (soif <= seuilCritique) return BesoinType.Soif;
        if (energie <= seuilCritique) return BesoinType.Energie;
        return BesoinType.Rien;
    }

    /// <summary>
    /// Notifie le script que le PNJ est arrivé à destination, déclenchant l’action correspondante.
    /// </summary>
    public void NotifieArrivee()
    {
        Transform target = GetComponent<PathfindingAI>().target;
        if (target == null)
        {
            Debug.LogWarning("La ressource a disparu avant l’arrivée. Retour à l’état Idle.");
            etatActuel = EtatPNJ.Idle;
            return;
        }

        switch (etatActuel)
        {
            case EtatPNJ.AllerManger:
                etatActuel = EtatPNJ.Manger;
                actionTimer = 0f;
                break;
            case EtatPNJ.AllerBoire:
                etatActuel = EtatPNJ.Boire;
                actionTimer = 0f;
                break;
            case EtatPNJ.AllerDormir:
                etatActuel = EtatPNJ.Dormir;
                actionTimer = 0f;
                break;
            case EtatPNJ.AllerTravailler:
                etatActuel = EtatPNJ.Travailler;
                actionTimer = 0f;
                break;
        }
    }

    /// <summary>Lance la recherche et le déplacement vers une ressource de type "baies".</summary>
    void Manger()
    {
        Debug.Log("Recherche d'un buisson pour manger...");
        StartCoroutine(AttendreEtChercher("baies", EtatPNJ.AllerManger));
    }

    /// <summary>Termine l'action de manger, remet la faim à 1 et détruit la ressource.</summary>
    void FinManger()
    {
        Transform cible = GetComponent<PathfindingAI>().target;
        faim = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        if (cible != null)
        {
            StartCoroutine(AttendreEtDetruire(cible.gameObject));
        }
        Debug.Log("Faim satisfaite, baie en cours de suppression.");
    }

    /// <summary>Lance la recherche et le déplacement vers une maison pour boire.</summary>
    void Boire()
    {
        Debug.Log("Recherche d'une maison pour boire...");
        StartCoroutine(AttendreEtChercher("Maison", EtatPNJ.AllerBoire));
    }

    /// <summary>Termine l'action de boire, remet la soif à 1.</summary>
    void FinBoire()
    {
        soif = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        LibererRessource();
        Debug.Log("Soif étanchée.");
    }

    /// <summary>Lance la recherche et le déplacement vers une maison pour dormir.</summary>
    void Dormir()
    {
        Debug.Log("Recherche d'une maison pour dormir...");
        StartCoroutine(AttendreEtChercher("Maison", EtatPNJ.AllerDormir));
    }

    /// <summary>Termine l'action de dormir, remet l'énergie à 1 et marque le logement comme acquis.</summary>
    void FinDormir()
    {
        energie = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        aUnLogement = true;
        LibererRessource();
        Debug.Log("Énergie restaurée.");
    }

    /// <summary>Recherche une usine aléatoire disponible pour aller travailler.</summary>
    void Travailler()
    {
        Debug.Log("Recherche d'une usine pour travailler...");
        string tagAleatoire = tagsUsines[Random.Range(0, tagsUsines.Length)];
        StartCoroutine(AttendreEtChercher(tagAleatoire, EtatPNJ.AllerTravailler));
    }

    /// <summary>Termine l'action de travail.</summary>
    void FinTravailler()
    {
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        LibererRessource();
        Debug.Log("Travail accompli.");
    }

    /// <summary>
    /// Cherche la cible la plus proche avec un tag donné, en vérifiant les disponibilités de la ressource.
    /// </summary>
    public Transform ChercherTarget(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        if (targets.Length == 0) return null;

        GameObject nearestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            var slot = obj.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot == null || !slot.EstDisponible()) continue;

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = obj;
            }
        }

        if (nearestTarget != null)
        {
            var slot = nearestTarget.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot != null && slot.VoirDisponibilite())
            {
                return nearestTarget.transform;
            }
        }

        return null;
    }

    /// <summary>
    /// Libère la ressource actuellement utilisée par le PNJ, si applicable.
    /// </summary>
    void LibererRessource()
    {
        Transform currentTarget = GetComponent<PathfindingAI>().target;
        if (currentTarget != null)
        {
            var slot = currentTarget.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot != null) slot.Liberer();
        }
    }

    /// <summary>
    /// Coroutine qui attend un court instant avant de chercher une ressource avec un tag donné.
    /// </summary>
    private System.Collections.IEnumerator AttendreEtChercher(string tag, EtatPNJ nouvelEtat)
    {
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));

        Transform target = ChercherTarget(tag);
        if (target != null)
        {
            RouteManager.Instance.AllerViaRoute(this.gameObject, target);
            etatActuel = nouvelEtat;
            Debug.Log($"En route vers {tag} via route...");
        }
        else
        {
            Debug.LogWarning($"Aucune ressource avec le tag {tag} trouvée !");
        }
    }

    /// <summary>
    /// Coroutine qui attend puis détruit un objet ressource après un court délai.
    /// </summary>
    private System.Collections.IEnumerator AttendreEtDetruire(GameObject cible)
    {
        yield return new WaitForSeconds(2f);
        if (cible != null)
        {
            var slot = cible.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot != null) slot.Liberer();

            Destroy(cible);
            Debug.Log("Baie détruite 2 secondes après l'arrivée.");
        }
    }
}
