using UnityEngine;

/// <summary>
/// Gère les besoins vitaux des PNJ : faim, soif, énergie.
/// Le PNJ choisit une action à effectuer selon ses besoins.
/// Il vieillit avec le temps et peut mourir naturellement.
/// </summary>
public class BesoinPlayers : MonoBehaviour
{
    /// <summary>Valeur de la soif du PNJ (1 = pleine, 0 = vide).</summary>
    public float soif = 1f, faim = 1f, energie = 1f;

    /// <summary>Vitesse de diminution de la faim, de la soif et de l'énergie.</summary>
    public float tauxFaim = 0.02f, tauxSoif = 0.01f, tauxEnergie = 0.01f;

    /// <summary>Type de besoin prioritaire du PNJ.</summary>
    public enum BesoinType { Rien, Faim, Soif, Energie }

    /// <summary>Seuil critique en-dessous duquel un besoin devient prioritaire.</summary>
    public float seuilCritique = 0.15f;

    /// <summary>Seuil d'action à partir duquel le PNJ commence à chercher une solution.</summary>
    public float seuilAction = 0.3f;
    public enum TachePNJ { Aucun, Bucheron, Carrier, OuvrierBois, OuvrierPierre, OuvrierArgile }
    public TachePNJ tache = TachePNJ.Aucun;

    /// <summary>État actuel du PNJ selon ses besoins ou son activité.</summary>
    public enum EtatPNJ { Idle, AllerManger, Manger, AllerBoire, Boire, AllerDormir, Dormir }
    private EtatPNJ etatActuel = EtatPNJ.Idle;

    private BesoinType besoinActuel;

    private float actionTimer = 0f;
    private float actionDuration = 2f;
    public float dureeDeVieMin = 120f;
    public float dureeDeVieMax = 240f;
    private float dureeDeVie;
    private float vieEcoulee = 0f;

    /// <summary>Référence au gestionnaire de population (pour notifier les décès).</summary>
    public PopulationManager populationManager;

    void Awake()
    {
        dureeDeVie = Random.Range(dureeDeVieMin, dureeDeVieMax);
    }

    void LateUpdate()
    {
        vieEcoulee += Time.deltaTime;
        if (vieEcoulee >= dureeDeVie)
        {
            Debug.Log($"{gameObject.name} est mort de vieillesse.");

            if (populationManager != null)
            {
                populationManager.PNJMort();
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        faim = Mathf.Clamp01(faim - tauxFaim * Time.deltaTime);
        soif = Mathf.Clamp01(soif - tauxSoif * Time.deltaTime);
        energie = Mathf.Clamp01(energie - tauxEnergie * Time.deltaTime);

        Debug.Log($"État actuel : {etatActuel} | Faim: {faim:F2} | Soif: {soif:F2} | Énergie: {energie:F2}");

        if (etatActuel == EtatPNJ.AllerManger || etatActuel == EtatPNJ.AllerBoire || etatActuel == EtatPNJ.AllerDormir)
        {
            var currentTarget = GetComponent<PathfindingAI>().target;
            if (currentTarget == null)
            {
                Debug.LogWarning("La ressource a été détruite avant l’arrivée. Retour à Idle.");
                etatActuel = EtatPNJ.Idle;
                return;
            }
        }
        if (etatActuel == EtatPNJ.Idle && GetBesoinPrioritaire() == BesoinType.Rien)
        {
            switch (tache)
            {
                case TachePNJ.Bucheron:
                    StartCoroutine(AttendreEtChercher("Woodcutter", EtatPNJ.AllerManger)); break;
                case TachePNJ.Carrier:
                    StartCoroutine(AttendreEtChercher("StoneQuarry", EtatPNJ.AllerManger)); break;
                case TachePNJ.OuvrierBois:
                    StartCoroutine(AttendreEtChercher("WoodToolFactory", EtatPNJ.AllerManger)); break;
                case TachePNJ.OuvrierPierre:
                    StartCoroutine(AttendreEtChercher("StoneToolFactory", EtatPNJ.AllerManger)); break;
                case TachePNJ.OuvrierArgile:
                    StartCoroutine(AttendreEtChercher("BrickFactory", EtatPNJ.AllerManger)); break;
            }
        }


        switch (etatActuel)
        {
            case EtatPNJ.Idle:
                besoinActuel = GetBesoinPrioritaire();
                if (besoinActuel != BesoinType.Rien)
                {
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
                    }
                }
                break;

            case EtatPNJ.Manger:
            case EtatPNJ.Boire:
            case EtatPNJ.Dormir:
                actionTimer += Time.deltaTime;
                if (actionTimer >= actionDuration)
                {
                    switch (etatActuel)
                    {
                        case EtatPNJ.Manger: FinManger(); break;
                        case EtatPNJ.Boire: FinBoire(); break;
                        case EtatPNJ.Dormir: FinDormir(); break;
                    }
                }
                break;
        }
    }

    /// <summary>Retourne le besoin prioritaire selon les valeurs actuelles.</summary>
    public BesoinType GetBesoinPrioritaire()
    {
        if (faim <= 0.15f) return BesoinType.Faim;
        if (soif <= 0.15f) return BesoinType.Soif;
        if (energie <= 0.15f) return BesoinType.Energie;
        return BesoinType.Rien;
    }
    public void PreparerSatisfaction()
    {
        if (etatActuel == EtatPNJ.AllerManger) faim = 1f;
        if (etatActuel == EtatPNJ.AllerBoire) soif = 1f;
        if (etatActuel == EtatPNJ.AllerDormir) energie = 1f;
    }

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
        }
    }

    void Manger()
    {
        Debug.Log("Recherche d'un buisson pour manger...");
        StartCoroutine(AttendreEtChercher("baies", EtatPNJ.AllerManger));
    }

    void FinManger()
    {
        faim = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;

        Transform target = GetComponent<PathfindingAI>().target;
        if (target != null)
        {
            StartCoroutine(AttendreEtDetruire(target.gameObject));
        }

        Debug.Log("Faim satisfaite, baie en cours de suppression.");
    }

    void Boire()
    {
        Debug.Log("Recherche d'une maison pour boire...");
        StartCoroutine(AttendreEtChercher("Maison", EtatPNJ.AllerBoire));
    }

    void FinBoire()
    {
        soif = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        LibererRessource();
        Debug.Log("Soif étanchée.");
    }

    void Dormir()
    {
        Debug.Log("Recherche d'une maison pour dormir...");
        StartCoroutine(AttendreEtChercher("Maison", EtatPNJ.AllerDormir));
    }

    void FinDormir()
    {
        energie = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        LibererRessource();
        Debug.Log("Énergie restaurée.");
    }

    /// <summary>Cherche la ressource la plus proche disponible avec le tag spécifié.</summary>
    public Transform ChercherTarget(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        if (targets.Length == 0) return null;

        GameObject bestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            var slot = obj.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot == null) continue;
            if (!slot.VoirDisponibilite()) continue;

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < minDistance)
            {
                if (bestTarget != null)
                {
                    var oldSlot = bestTarget.GetComponent<RessourceMaxPlayerCapacity>();
                    if (oldSlot != null) oldSlot.Liberer();
                }

                bestTarget = obj;
                minDistance = distance;
            }
            else
            {
                slot.Liberer();
            }
        }

        return bestTarget != null ? bestTarget.transform : null;
    }

    /// <summary>Libère la ressource actuellement ciblée par le PNJ.</summary>
    void LibererRessource()
    {
        Transform currentTarget = GetComponent<PathfindingAI>().target;
        if (currentTarget != null)
        {
            var slot = currentTarget.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot != null) slot.Liberer();
        }
    }

    private System.Collections.IEnumerator AttendreEtChercher(string tag, EtatPNJ nouvelEtat)
    {
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));

        Transform target = ChercherTarget(tag);
        if (target != null)
        {
            GetComponent<PathfindingAI>().setTarget(target);
            etatActuel = nouvelEtat;
            Debug.Log($"En route vers {tag}...");
            FinManger();
            FinBoire();
            FinDormir();
        }
        else
        {
            FinManger();
            FinBoire();
            FinDormir();
            Debug.LogWarning($"Aucune ressource avec le tag {tag} trouvée !");
        }
    }

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

    /// <summary>Instancie un nouveau PNJ dans la scène à la position donnée.</summary>
    public static void CreerNouveauPNJ(GameObject prefabPNJ, Vector3 position)
    {
        GameObject nouveauPNJ = GameObject.Instantiate(prefabPNJ, position, Quaternion.identity);
        Debug.Log("👶 Nouveau PNJ créé !");
    }
}
