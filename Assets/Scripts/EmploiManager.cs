using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère l'affichage du nombre d'emplois disponibles et occupés dans le jeu.
/// Se base sur les objets actifs dans la scène ayant des tags d'usine spécifiques et un composant <see cref="RessourceMaxPlayerCapacity"/>.
/// </summary>
public class EmploiManager : MonoBehaviour
{
    /// <summary>
    /// Composant chargé de l'affichage des statistiques à l'écran.
    /// </summary>
    public StatistiquesManager statDisplay;

    /// <summary>
    /// Tags correspondant aux bâtiments considérés comme lieux de travail.
    /// </summary>
    private readonly string[] tagsUsines = {
        "usineBois", "usineOutils", "usinePierre", "usineOutilsPierre",
        "usineArgile", "potterie", "usineBrique", "usineEau", "usineBaie",
        "enclotMouton", "enclotPoule"
    };

    /// <summary>
    /// Lance la mise à jour périodique des statistiques à l'initialisation.
    /// </summary>
    void Start()
    {
        StartCoroutine(MiseAJourPeriodique());
    }

    /// <summary>
    /// Coroutine qui met à jour régulièrement l'affichage des emplois.
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
    /// Met à jour les statistiques d'affichage en comptant les postes de travail actifs.
    /// </summary>
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

    /// <summary>
    /// Met à jour manuellement l'affichage des postes (utile après une modification).
    /// </summary>
    public void RafraichirListePostes()
    {
        MettreAJourAffichage();
    }

    /// <summary>
    /// Retourne tous les bâtiments actifs dans la scène considérés comme postes de travail (usines).
    /// </summary>
    /// <returns>Tableau de <see cref="RessourceMaxPlayerCapacity"/> représentant les postes actifs.</returns>
    RessourceMaxPlayerCapacity[] GetPostesTravailActifs()
    {
        RessourceMaxPlayerCapacity[] tous = GameObject.FindObjectsOfType<RessourceMaxPlayerCapacity>(true);
        List<RessourceMaxPlayerCapacity> resultats = new List<RessourceMaxPlayerCapacity>();

        foreach (var poste in tous)
        {
            GameObject obj = poste.gameObject;

            if (!obj.activeInHierarchy) continue; // Ignore objets désactivés
            if (obj.GetComponentInParent<Canvas>() != null) continue; // Ignore UI
            if (!EstUsine(obj)) continue; // Doit avoir un tag d'usine

            resultats.Add(poste);
        }

        return resultats.ToArray();
    }

    /// <summary>
    /// Vérifie si un GameObject possède un tag correspondant à une usine.
    /// </summary>
    /// <param name="obj">GameObject à tester.</param>
    /// <returns>True s'il s'agit d'une usine, sinon False.</returns>
    bool EstUsine(GameObject obj)
    {
        foreach (string tag in tagsUsines)
        {
            if (obj.CompareTag(tag)) return true;
        }
        return false;
    }
}
