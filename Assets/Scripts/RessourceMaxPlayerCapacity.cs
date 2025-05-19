using UnityEngine;

public class RessourceMaxPlayerCapacity : MonoBehaviour
{
    public int capaciteMax = 1;
    private int occupationActuelle = 0;

    public bool VoirDisponibilite()
    {
        if (occupationActuelle < capaciteMax)
        {
            occupationActuelle++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Liberer()
    {
        if (occupationActuelle > 0)
        {
            occupationActuelle--;
        }
    }

    public bool EstDisponible()
    {
        return occupationActuelle < capaciteMax;
    }
}
