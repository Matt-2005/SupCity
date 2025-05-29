using UnityEngine;
using System.Collections;

public class BesoinPlayers : MonoBehaviour
{
    public float soif = 1f, faim = 1f, energie = 1f;
    public float tauxFaim = 0.02f, tauxSoif = 0.01f, tauxEnergie = 0.01f;

    public enum BesoinType { Rien, Faim, Soif, Energie }
    public enum EtatPNJ { Idle, AllerManger, Manger, AllerBoire, Boire, AllerDormir, Dormir, AllerTravailler, Travailler, AllerSeDetendre, SeDetendre }
    public enum TachePNJ { Aucun, Bucheron, Carrier, OuvrierBois, OuvrierPierre, OuvrierArgile }

    public TachePNJ tache = TachePNJ.Aucun;
    private EtatPNJ etatActuel = EtatPNJ.Idle;
    private BesoinType besoinActuel;

    private float actionTimer = 0f;
    private float actionDuration = 2f;
    public float dureeDeVieMin = 120f;
    public float dureeDeVieMax = 240f;
    private float dureeDeVie;
    private float vieEcoulee = 0f;

    public PopulationManager populationManager;
    private Vector3 pointDeDepart;

    void Awake()
    {
        dureeDeVie = Random.Range(dureeDeVieMin, dureeDeVieMax);
    }

    void Start()
    {
        pointDeDepart = transform.position;
    }

    void LateUpdate()
    {
        vieEcoulee += Time.deltaTime;
        if (vieEcoulee >= dureeDeVie)
        {
            Debug.Log($"{gameObject.name} est mort de vieillesse.");
            if (populationManager != null) populationManager.PNJMort();
            Destroy(gameObject);
        }
    }

    void Update()
    {
        faim = Mathf.Clamp01(faim - tauxFaim * Time.deltaTime);
        soif = Mathf.Clamp01(soif - tauxSoif * Time.deltaTime);
        energie = Mathf.Clamp01(energie - tauxEnergie * Time.deltaTime);

        switch (etatActuel)
        {
            case EtatPNJ.Idle:
                besoinActuel = GetBesoinPrioritaire();
                if (besoinActuel != BesoinType.Rien)
                {
                    switch (besoinActuel)
                    {
                        case BesoinType.Faim: Manger(); break;
                        case BesoinType.Soif: Boire(); break;
                        case BesoinType.Energie: Dormir(); break;
                    }
                }
                else
                {
                    switch (tache)
                    {
                        case TachePNJ.Bucheron: StartCoroutine(AttendreEtChercher("Woodcutter", EtatPNJ.AllerTravailler)); break;
                        case TachePNJ.Carrier: StartCoroutine(AttendreEtChercher("StoneQuarry", EtatPNJ.AllerTravailler)); break;
                        case TachePNJ.OuvrierBois: StartCoroutine(AttendreEtChercher("WoodToolFactory", EtatPNJ.AllerTravailler)); break;
                        case TachePNJ.OuvrierPierre: StartCoroutine(AttendreEtChercher("StoneToolFactory", EtatPNJ.AllerTravailler)); break;
                        case TachePNJ.OuvrierArgile: StartCoroutine(AttendreEtChercher("BrickFactory", EtatPNJ.AllerTravailler)); break;
                        case TachePNJ.Aucun: StartCoroutine(SeDetendreAutour()); break;
                    }
                }
                break;

            case EtatPNJ.Manger:
            case EtatPNJ.Boire:
            case EtatPNJ.Dormir:
            case EtatPNJ.Travailler:
            case EtatPNJ.SeDetendre:
                actionTimer += Time.deltaTime;
                if (actionTimer >= actionDuration)
                {
                    switch (etatActuel)
                    {
                        case EtatPNJ.Manger: FinManger(); break;
                        case EtatPNJ.Boire: FinBoire(); break;
                        case EtatPNJ.Dormir: FinDormir(); break;
                        case EtatPNJ.Travailler: RetourMaison(); break;
                        case EtatPNJ.SeDetendre: etatActuel = EtatPNJ.Idle; break;
                    }
                }
                break;
        }
    }

    public BesoinType GetBesoinPrioritaire()
    {
        if (faim <= 0.15f) return BesoinType.Faim;
        if (soif <= 0.15f) return BesoinType.Soif;
        if (energie <= 0.15f) return BesoinType.Energie;
        return BesoinType.Rien;
    }

    public void NotifieArrivee()
    {
        Transform target = GetComponent<PathfindingAI>().target;
        if (target == null) { etatActuel = EtatPNJ.Idle; return; }

        switch (etatActuel)
        {
            case EtatPNJ.AllerManger: etatActuel = EtatPNJ.Manger; break;
            case EtatPNJ.AllerBoire: etatActuel = EtatPNJ.Boire; break;
            case EtatPNJ.AllerDormir: etatActuel = EtatPNJ.Dormir; break;
            case EtatPNJ.AllerTravailler: etatActuel = EtatPNJ.Travailler; break;
            case EtatPNJ.AllerSeDetendre: etatActuel = EtatPNJ.SeDetendre; break;
        }
        actionTimer = 0f;
    }

    void Manger() => StartCoroutine(AttendreEtChercher("usineBaie", EtatPNJ.AllerManger));
    void Boire() => StartCoroutine(AttendreEtChercher("Maison", EtatPNJ.AllerBoire));
    void Dormir() => StartCoroutine(AttendreEtChercher("Maison", EtatPNJ.AllerDormir));

    void FinManger()
    {
        faim = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        Transform target = GetComponent<PathfindingAI>().target;
        if (target != null) StartCoroutine(AttendreEtDetruire(target.gameObject));
    }

    void FinBoire()
    {
        soif = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        LibererRessource();
    }

    void FinDormir()
    {
        energie = 1f;
        etatActuel = EtatPNJ.Idle;
        actionTimer = 0f;
        LibererRessource();
    }

    void RetourMaison()
    {
        StartCoroutine(AttendreEtChercher("Maison", EtatPNJ.AllerDormir));
    }

    Transform ChercherTarget(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        if (targets.Length == 0) return null;

        GameObject best = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            var slot = obj.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot == null || !slot.VoirDisponibilite()) continue;

            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                if (best != null) best.GetComponent<RessourceMaxPlayerCapacity>()?.Liberer();
                best = obj;
                minDist = dist;
            }
            else slot.Liberer();
        }

        return best != null ? best.transform : null;
    }

    void LibererRessource()
    {
        Transform t = GetComponent<PathfindingAI>().target;
        if (t != null)
        {
            var slot = t.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot != null) slot.Liberer();
        }
    }

    IEnumerator AttendreEtChercher(string tag, EtatPNJ nouvelEtat)
    {
        yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        Transform target = ChercherTarget(tag);
        if (target != null)
        {
            GetComponent<PathfindingAI>().setTarget(target);
            etatActuel = nouvelEtat;
        }
    }

    IEnumerator AttendreEtDetruire(GameObject cible)
    {
        yield return new WaitForSeconds(2f);
        if (cible != null)
        {
            var slot = cible.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot != null) slot.Liberer();
            Destroy(cible);
        }
    }

    IEnumerator SeDetendreAutour()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 point = pointDeDepart + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);
        GameObject temp = new GameObject("TargetRelax");
        temp.transform.position = point;
        GetComponent<PathfindingAI>().setTarget(temp.transform);
        etatActuel = EtatPNJ.AllerSeDetendre;
        Destroy(temp, 5f);
    }

    public static void CreerNouveauPNJ(GameObject prefabPNJ, Vector3 position)
    {
        GameObject nouveau = Instantiate(prefabPNJ, position, Quaternion.identity);
        Debug.Log("👶 Nouveau PNJ créé !");
    }
}
