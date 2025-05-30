using TMPro;
using UnityEngine;

public class StatistiquesManager : MonoBehaviour
{
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI habitationsText;
    public TextMeshProUGUI emploisText;
    public TextMeshProUGUI progressionText;

    public void SetPopulation(int value)
    {
        if (populationText != null)
            populationText.text = "Population : " + value;
    }

    public void SetHabitations(int occupes, int capaciteTotale)
    {
        if (habitationsText != null)
            habitationsText.text = "Habitations : " + occupes + " / " + capaciteTotale;
    }

    public void SetEmplois(int occupes, int disponibles)
    {
        if (emploisText != null)
            emploisText.text = $"Emplois : {occupes} / {disponibles}";
    }

    public void SetProgression(float pourcentage)
    {
        if (progressionText != null)
            progressionText.text = "Progression : " + Mathf.RoundToInt(pourcentage * 100f) + "%";
    }
}
