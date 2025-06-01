using TMPro;
using UnityEngine;

/// <summary>
/// Gère l'affichage des différentes statistiques du jeu dans l'interface utilisateur.
/// Utilise TextMeshProUGUI pour afficher les textes à l'écran.
/// </summary>
public class StatistiquesManager : MonoBehaviour
{
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI habitationsText;
    public TextMeshProUGUI emploisText;
    public TextMeshProUGUI progressionText;

    /// <summary>
    /// Met à jour le texte affichant la population actuelle.
    /// </summary>
    /// <param name="value">Nombre total de PNJ dans le jeu.</param>
    public void SetPopulation(int value)
    {
        if (populationText != null)
            populationText.text = "Population : " + value;
    }

    /// <summary>
    /// Met à jour l'affichage du nombre de personnes logées sur la capacité totale.
    /// </summary>
    /// <param name="occupes">Nombre de places occupées.</param>
    /// <param name="capaciteTotale">Capacité totale de logement disponible.</param>
    public void SetHabitations(int occupes, int capaciteTotale)
    {
        if (habitationsText != null)
            habitationsText.text = "Habitations : " + occupes + " / " + capaciteTotale;
    }

    /// <summary>
    /// Met à jour l'affichage des emplois occupés sur le total disponible.
    /// </summary>
    /// <param name="occupes">Nombre de postes actuellement occupés.</param>
    /// <param name="disponibles">Nombre total de postes disponibles.</param>
    public void SetEmplois(int occupes, int disponibles)
    {
        if (emploisText != null)
            emploisText.text = $"Emplois : {occupes} / {disponibles}";
    }

    /// <summary>
    /// Met à jour l'affichage de la progression du joueur en pourcentage.
    /// </summary>
    /// <param name="pourcentage">Valeur comprise entre 0.0 et 1.0 représentant la progression.</param>
    public void SetProgression(float pourcentage)
    {
        if (progressionText != null)
            progressionText.text = "Progression : " + Mathf.RoundToInt(pourcentage * 100f) + "%";
    }
}
