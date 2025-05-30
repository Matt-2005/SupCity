using UnityEngine;

public class BesoinPlayers : MonoBehaviour
{
    public float soif = 1f, faim = 1f, energie = 1f;
    public float tauxFaim = 0.02f, tauxSoif = 0.01f, tauxEnergie = 0.01f;

    public enum BesoinType { Rien, Faim, Soif, Energie }
    public float seuilCritique = 0.15f;
    public float seuilAction = 0.3f;

    public enum EtatPNJ { Idle, AllerManger, Manger, AllerBoire, Boire, AllerDormir, Dormir }
    private EtatPNJ etatActuel = EtatPNJ.Idle;

    private bool enAction = false;
    private bool estChezSoi = false;
    private BesoinType besoinActuel;

    private float actionTimer = 0f;
    private float actionDuration = 2f;

    void Update()
    {
        faim = Mathf.Clamp01(faim - tauxFaim * Time.deltaTime);
        soif = Mathf.Clamp01(soif - tauxSoif * Time.deltaTime);
        energie = Mathf.Clamp01(energie - tauxEnergie * Time.deltaTime);

        Debug.Log($"État actuel : {etatActuel} | Faim: {faim:F2} | Soif: {soif:F2} | Énergie: {energie:F2}");

        // 🔒 Vérifie si la cible a été supprimée
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

        // Réserver la ressource trouvée
        if (nearestTarget != null)
        {
            var slot = nearestTarget.GetComponent<RessourceMaxPlayerCapacity>();
            if (slot != null && slot.VoirDisponibilite()) // ✅ ici on réserve
            {
                return nearestTarget.transform;
            }
        }

        return null;
    }


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
        }
        else
        {
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




}