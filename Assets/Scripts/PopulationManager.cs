using UnityEngine;
using System.Collections;

/// <summary>
/// Gère la génération automatique de PNJ dans la scène.
/// S'assure que la population ne dépasse pas un maximum défini.
/// Met à jour l'affichage via <see cref="StatistiquesManager"/>.
/// </summary>
public class PopulationManager : MonoBehaviour
{
    /// <summary>Instance unique du gestionnaire de population (Singleton).</summary>
    public static PopulationManager Instance;

    /// <summary>Prefab du PNJ à instancier.</summary>
    public GameObject prefabPNJ;

    /// <summary>Temps entre chaque naissance (en secondes).</summary>
    public float intervalleNaissance = 5f;

    /// <summary>Population maximale autorisée.</summary>
    public int populationMax = 100;

    /// <summary>Population actuelle dans la scène.</summary>
    private int populationActuelle = 0;

    /// <summary>Position fixe de génération des PNJ.</summary>
    private Vector3 spawnPosition = new Vector3(-20f, 22f, 0f);

    /// <summary>Composant responsable de l'affichage des statistiques.</summary>
    public StatistiquesManager statDisplay;

    /// <summary>Initialise l’instance Singleton ou détruit les doublons.</summary>
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

    /// <summary>
    /// Initialise la population en comptant les PNJ présents dans la scène.
    /// Démarre ensuite la génération automatique.
    /// </summary>
    void Start()
    {
        populationActuelle = GameObject.FindObjectsOfType<BesoinPlayers>().Length;
        MettreAJourAffichage();

        StartCoroutine(GenererPNJ());
    }

    /// <summary>
    /// Coroutine qui génère un PNJ à intervalles réguliers selon le timeScale.
    /// Ne dépasse jamais <see cref="populationMax"/>.
    /// </summary>
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

    /// <summary>
    /// Met à jour l'affichage de la population actuelle via l’interface.
    /// </summary>
    private void MettreAJourAffichage()
    {
        if (statDisplay != null)
        {
            statDisplay.SetPopulation(populationActuelle);
        }
    }

    /// <summary>
    /// Retourne le nombre de PNJ actuellement présents dans la scène.
    /// </summary>
    /// <returns>Population actuelle.</returns>
    public int GetPopulation()
    {
        return populationActuelle;
    }
}
