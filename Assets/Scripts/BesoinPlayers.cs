using UnityEngine;

public class BesoinPlayers : MonoBehaviour
{
    public float soif = 1f, faim = 1f, energie = 1f;
    public float tauxFaim = 0.02f, tauxSoif = 0.01f, tauxEnergie = 0.01f;
    public enum BesoinType { Rien, Faim, Soif, Energie }
    public float seuilCritique = 0.15f;
    public float seuilAction = 0.3f;
    public BesoinType GetBesoinPrioritaire()
    {
        // Priorité basée sur des seuils
        if (faim <= 0.15f) return BesoinType.Faim;
        if (soif <= 0.15f) return BesoinType.Soif;
        if (energie <= 0.15f) return BesoinType.Energie;
        return BesoinType.Rien;
    }
    private bool enAction = false; // Initialisé à false
    private bool estChezSoi = false;
    private BesoinType besoinActuel;
    
   void Update()
    {
        faim = Mathf.Clamp01(faim - tauxFaim * Time.deltaTime);
        soif = Mathf.Clamp01(soif - tauxSoif * Time.deltaTime);
        energie = Mathf.Clamp01(energie - tauxEnergie * Time.deltaTime);

        // Vérifier le besoin prioritaire
        besoinActuel = GetBesoinPrioritaire();

        if (!enAction)
        {
            switch(besoinActuel)
            {
                case BesoinType.Faim:
                    GererFaim();
                    break;
                    
                case BesoinType.Soif:
                    GererSoif();
                    break;
                    
                case BesoinType.Energie:
                    GererEnergie();
                    break;
                    
                case BesoinType.Rien:
                    if (estChezSoi) 
                    {
                        Debug.Log("Repos à domicile - tous besoins satisfaits");
                        
                    }
                    else
                    {
                        Debug.Log("En déplacement - aucun besoin urgent");
                    }
                    break;
            }
        }
    }


    void GererFaim()
    {
        if (faim <= seuilCritique)
        {
            if (!estChezSoi)
            {
                Debug.Log("Faim critique! Retour à la maison");
                RetournerChezSoi();
                Manger();
            }
            else
            {
                Debug.Log("Attente à la maison (faim < 15%)");
                Manger();
            }
        }
        else if (faim <= seuilAction)
        {
            if (estChezSoi)
            {
                Debug.Log("Manger à la maison");
                Manger();
            }
            else
            {
                Debug.Log("Aller à la maison pour manger");
                RetournerChezSoi();
            }
        }
    }

    void GererSoif()
    {
        if (soif <= seuilCritique)
        {
            if (!estChezSoi)
            {
                Debug.Log("Soif critique! Retour à la maison");
                RetournerChezSoi();
                Boire();
            }
            else
            {
                Debug.Log("Attente à la maison (soif < 15%)");
                Boire();
            }
        }
        else if (soif <= seuilAction)
        {
            if (estChezSoi)
            {
                Debug.Log("Boire à la maison");
                Boire();
            }
            else
            {
                Debug.Log("Aller à la maison pour boire");
                RetournerChezSoi();
            }
        }
    }

    void GererEnergie()
    {
        if (energie <= seuilCritique)
        {
            if (!estChezSoi)
            {
                Debug.Log("Fatigue critique! Retour à la maison");
                RetournerChezSoi();
                Dormir();
            }
            else
            {
                Debug.Log("Attente à la maison (énergie < 15%)");
                Dormir();
            }
        }
        else if (energie <= seuilAction)
        {
            if (estChezSoi)
            {
                Debug.Log("Dormir à la maison");
                Dormir();
            }
            else
            {
                Debug.Log("Aller à la maison pour dormir");
                RetournerChezSoi();
            }
        }
    }

    void RetournerChezSoi()
    {
        Debug.Log("En chemin vers la maison...");
        Invoke("ArriverChezSoi", 5f);
    }

    void ArriverChezSoi()
    {
        estChezSoi = true;
        Debug.Log("Arrivé à la maison");
    }

    void Manger()
    {
        enAction = true;
        Debug.Log("Manger en cours...");
        FinManger();
    }

    void FinManger()
    {
        faim = faim + 1f;
        enAction = false;
        Debug.Log("Faim satisfaite");
    }

    void Boire()
    {
        enAction = true;
        Debug.Log("Boire en cours...");
        FinBoire();
    }

    void FinBoire()
    {
        soif = soif + 1f;
        enAction = false;
        Debug.Log("Soif étanchée");
    }

    void Dormir()
    {
        enAction = true;
        Debug.Log("Dormir en cours...");
        FinDormir();
    }

    void FinDormir()
    {
        energie = energie + 1f;
        enAction = false;
        estChezSoi = false; // Quitte la maison après avoir dormi
        Debug.Log("Energie restaurée");
    }
   
}