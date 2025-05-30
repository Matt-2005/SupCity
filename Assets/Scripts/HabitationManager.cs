using UnityEngine;
using System.Collections;

public class HabitationManager : MonoBehaviour
{
    public StatistiquesManager statDisplay;

    void Start()
    {
        StartCoroutine(MiseAJourPeriodique());
    }

    IEnumerator MiseAJourPeriodique()
    {
        while (true)
        {
            MettreAJourAffichage();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void MettreAJourAffichage()
    {
        int capaciteTotale = 0;
        int personnesLogees = 0;

        var logements = GameObject.FindGameObjectsWithTag("Maison");

        foreach (var logement in logements)
        {
            if (!logement.activeInHierarchy) continue;

            var capacite = logement.GetComponent<RessourceMaxPlayerCapacity>();
            if (capacite == null) continue;

            capaciteTotale += capacite.capaciteMax;
            personnesLogees += capacite.OccupationActuelle;
        }

        statDisplay?.SetHabitations(personnesLogees, capaciteTotale);
    }
}
