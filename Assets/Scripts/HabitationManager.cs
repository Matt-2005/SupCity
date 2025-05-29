using UnityEngine;
using System.Collections;

public class HabitationManager : MonoBehaviour
{
    public StatistiquesManager statDisplay;
    private RessourceMaxPlayerCapacity[] logements;

    void Start()
    {
        logements = FindObjectsOfType<RessourceMaxPlayerCapacity>();

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
        int placesOccupees = 0;

        foreach (var logement in logements)
        {
            capaciteTotale += logement.capaciteMax;
            placesOccupees += logement.OccupationActuelle;
        }

        if (statDisplay != null)
        {
            statDisplay.SetHabitations(placesOccupees, capaciteTotale);
        }
    }
    
    public void RafraichirListeLogements()
    {
        logements = FindObjectsOfType<RessourceMaxPlayerCapacity>();
    }
}
