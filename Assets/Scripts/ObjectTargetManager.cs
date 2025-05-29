using System.Collections.Generic;
using UnityEngine;

public class ObjectTargetManager : MonoBehaviour
{
    public List<GameObject> baieDisponibles = new List<GameObject>();

    void Start()
    {
        GameObject[] baies = GameObject.FindGameObjectsWithTag("usineBaie");

        foreach (GameObject baie in baies)
        {
            baieDisponibles.Add(baie);
        }
    }

    void Update()
    {
        
    }
}
