using UnityEngine;
using System.Collections;

/// <summary>
/// Gère l'affichage du nombre total de places disponibles dans les maisons et le nombre de personnes logées.
/// Actualise régulièrement les statistiques en analysant les GameObjects ayant le tag "Maison".
/// </summary>
public class HabitationManager : MonoBehaviour
{
    /// <summary>
    /// Référence vers l'afficheur de statistiques (UI).
    /// </summary>
    public StatistiquesManager statDisplay;

    /// <summary>
    /// Démarre la mise à jour automatique des statistiques d'habitations.
    /// </summary>
    void Start()
    {
        StartCoroutine(MiseAJourPeriodique());
    }

    /// <summary>
    /// Coroutine qui met à jour les statistiques toutes les 0,5 secondes.
    /// </summary>
    IEnumerator MiseAJourPeriodique()
    {
        while (true)
        {
            MettreAJourAffichage();
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Calcule et affiche le nombre total de places et de personnes logées.
    /// Se base sur les objets avec le tag "Maison" et un composant <see cref="RessourceMaxPlayerCapacity"/>.
    /// </summary>
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
