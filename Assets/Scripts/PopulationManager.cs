using UnityEngine;
using System.Collections;

public class PopulationManager : MonoBehaviour
{
    public GameObject prefabPNJ;

    public float intervalleNaissance = 5f;
    public int populationMax = 1000;
    private int populationActuelle = 0;

    private Vector3 spawnPosition = new Vector3(-20f, 22f, 0f);

    void Start()
    {
        // Compte les PNJ déjà présents
        populationActuelle = GameObject.FindObjectsOfType<BesoinPlayers>().Length;

        // Associe le manager aux PNJ existants
        foreach (BesoinPlayers p in GameObject.FindObjectsOfType<BesoinPlayers>())
        {
            p.populationManager = this;
        }

        StartCoroutine(GenererPNJ());
    }

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

    public void PNJMort()
    {
        populationActuelle = Mathf.Max(0, populationActuelle - 1);
        Debug.Log($"💀 PNJ mort. Population : {populationActuelle}");
    }
}
