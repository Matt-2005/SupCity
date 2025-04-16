using System.Collections.Generic;
using UnityEngine;

public class ObjectTargetManager : MonoBehaviour
{

    public List<GameObject> arbresDisponibles = new List<GameObject>();
    void Start()
    {
        GameObject[] arbres = GameObject.FindGameObjectsWithTag("Arbre");

        foreach (GameObject arbre in arbres)
        {
            arbresDisponibles.Add(arbre);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
