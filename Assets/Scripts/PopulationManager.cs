using UnityEngine;
using System.Collections;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager Instance;
    public GameObject prefabPNJ;
    public float intervalleNaissance = 5f;
    public int populationMax = 100;
    private int populationActuelle = 0;

    private Vector3 spawnPosition = new Vector3(-20f, 22f, 0f);

    public StatistiquesManager statDisplay;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Compte les PNJ déjà présents dans la scène
        populationActuelle = GameObject.FindObjectsOfType<BesoinPlayers>().Length;
        MettreAJourAffichage();

        StartCoroutine(GenererPNJ());
    }

    IEnumerator GenererPNJ()
    {
        while (true)
        {
            float facteur = Mathf.Max(0.1f, Time.timeScale);
            yield return new WaitForSeconds(intervalleNaissance / facteur);

            if (populationActuelle >= populationMax) continue;

            GameObject nouveau = Instantiate(prefabPNJ, spawnPosition, Quaternion.identity);
            nouveau.name = $"Players ({populationActuelle})";

            populationActuelle++;
            MettreAJourAffichage();

            Debug.Log($"👶 Nouveau PNJ généré : {nouveau.name}");
        }
    }

    private void MettreAJourAffichage()
    {
        if (statDisplay != null)
        {
            statDisplay.SetPopulation(populationActuelle);
        }
    }

    public int GetPopulation()
    {
        return populationActuelle;
    }
}
