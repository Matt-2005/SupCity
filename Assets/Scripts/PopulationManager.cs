using UnityEngine;
using System.Collections;

/// <summary>
/// Gère la population de PNJ dans le jeu.
/// Fait naître régulièrement de nouveaux PNJ jusqu’à une limite maximale.
/// Gère également la notification de décès des PNJ.
/// </summary>
public class PopulationManager : MonoBehaviour
{
    /// <summary>Prefab utilisé pour instancier de nouveaux PNJ.</summary>
    public GameObject prefabPNJ;
    public float intervalleNaissance = 5f;
    public int populationMax = 1000;
    private int populationActuelle = 0;

    /// <summary>Position de spawn des nouveaux PNJ.</summary>
    private Vector3 spawnPosition = new Vector3(-20f, 22f, 0f);
    
    public StatistiquesManager statDisplay;

    /// <summary>
    /// Initialise le manager : compte les PNJ existants et leur assigne le manager.
    /// Lance ensuite la coroutine de génération.
    /// </summary>
    void Start()
    {
        populationActuelle = GameObject.FindObjectsOfType<BesoinPlayers>().Length;

        foreach (BesoinPlayers p in GameObject.FindObjectsOfType<BesoinPlayers>())
        {
            p.populationManager = this;
        }
        MettreAJourAffichage();
        StartCoroutine(GenererPNJ());
    }

    /// <summary>
    /// Coroutine qui fait naître un nouveau PNJ à intervalle régulier si la population le permet.
    /// </summary>
    IEnumerator GenererPNJ()
    {
        while (true)
        {
            // Sécurité pour éviter division par 0
            float facteur = Mathf.Max(0.1f, Time.timeScale);
            float attente = intervalleNaissance / facteur;

            yield return new WaitForSeconds(attente);

            if (populationActuelle < populationMax)
            {
                GameObject nouveau = Instantiate(prefabPNJ, spawnPosition, Quaternion.identity);

                // ✅ Renommer pour éviter "Clone"
                nouveau.name = "Players (" + populationActuelle + ")";

                populationActuelle++;
                Debug.Log($"👶 PNJ né ! Population : {populationActuelle}");

                BesoinPlayers besoin = nouveau.GetComponent<BesoinPlayers>();
                if (besoin != null)
                {
                    besoin.populationManager = this;
                }
                MettreAJourAffichage();
            }
        }
    }

    /// <summary>
    /// Appelé lorsqu’un PNJ meurt. Diminue le compteur de population.
    /// </summary>
    public void PNJMort()
    {
        populationActuelle = Mathf.Max(0, populationActuelle - 1);
        Debug.Log($"💀 PNJ mort. Population : {populationActuelle}");
        MettreAJourAffichage();
    }

    private void MettreAJourAffichage()
    {
        if (statDisplay != null)
        {
            statDisplay.SetPopulation(populationActuelle);
        }
    }

}