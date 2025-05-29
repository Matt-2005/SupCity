using UnityEngine;
using System.Collections;

public class PopulationManager : MonoBehaviour
{
    public GameObject prefabPNJ;
    public float intervalleNaissance = 5f;
    public int populationMax = 1000;
    private int populationActuelle = 0;

    private Vector3 spawnPosition = new Vector3(-20f, 22f, 0f);
    
    public StatistiquesManager statDisplay;

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