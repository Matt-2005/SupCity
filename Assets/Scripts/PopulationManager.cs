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

    /// <summary>Intervalle de temps entre chaque naissance (en secondes).</summary>
    public float intervalleNaissance = 10f;

    /// <summary>Nombre maximal de PNJ autorisés dans la scène.</summary>
    public int populationMax = 50;

    /// <summary>Nombre actuel de PNJ vivants.</summary>
    private int populationActuelle = 0;

    /// <summary>Position de spawn des nouveaux PNJ.</summary>
    private Vector3 spawnPosition = new Vector3(-20f, 22f, 0f);

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

        StartCoroutine(GenererPNJ());
    }

    /// <summary>
    /// Coroutine qui fait naître un nouveau PNJ à intervalle régulier si la population le permet.
    /// </summary>
    IEnumerator GenererPNJ()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalleNaissance);

            if (populationActuelle < populationMax)
            {
                GameObject nouveau = Instantiate(prefabPNJ, spawnPosition, Quaternion.identity);
                populationActuelle++;
                Debug.Log($"👶 PNJ né ! Population : {populationActuelle}");

                BesoinPlayers besoin = nouveau.GetComponent<BesoinPlayers>();
                if (besoin != null)
                {
                    besoin.populationManager = this;
                }
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
    }
}
