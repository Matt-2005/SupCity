using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère l'affichage des emplois occupés et disponibles.
/// Se base sur les objets actifs dans la scène ayant un tag d'usine et un composant RessourceMaxPlayerCapacity.
/// </summary>
public class EmploiManager : MonoBehaviour
{
    public StatistiquesManager statDisplay;

    // Tags correspondant aux bâtiments considérés comme lieux de travail
    private readonly string[] tagsUsines = {
        "usineBois", "usineOutils", "usinePierre", "usineOutilsPierre",
        "usineArgile", "potterie", "usineBrique", "usineEau", "usineBaie",
        "enclotMouton", "enclotPoule"
    };

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
        var postes = GetPostesTravailActifs();

        int totalCapacite = 0;
        int emploisOccupes = 0;

        foreach (var poste in postes)
        {
            totalCapacite += poste.capaciteMax;
            emploisOccupes += poste.OccupationActuelle;
        }

        statDisplay?.SetEmplois(emploisOccupes, totalCapacite);
    }

    public void RafraichirListePostes()
    {
        MettreAJourAffichage();
    }

    /// <summary>
    /// Retourne tous les bâtiments actifs dans la scène considérés comme postes de travail.
    /// </summary>
    RessourceMaxPlayerCapacity[] GetPostesTravailActifs()
    {
        RessourceMaxPlayerCapacity[] tous = GameObject.FindObjectsOfType<RessourceMaxPlayerCapacity>(true);
        List<RessourceMaxPlayerCapacity> resultats = new List<RessourceMaxPlayerCapacity>();

        foreach (var poste in tous)
        {
            GameObject obj = poste.gameObject;

            if (!obj.activeInHierarchy) continue; // Ignore désactivés
            if (obj.GetComponentInParent<Canvas>() != null) continue; // Ignore UI
            if (!EstUsine(obj)) continue; // Doit être une usine

            resultats.Add(poste);
        }

        return resultats.ToArray();
    }

    bool EstUsine(GameObject obj)
    {
        foreach (string tag in tagsUsines)
        {
            if (obj.CompareTag(tag)) return true;
        }
        return false;
    }
}
