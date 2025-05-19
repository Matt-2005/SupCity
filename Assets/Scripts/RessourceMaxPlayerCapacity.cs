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
        return false;
    }

    public void Liberer()
    {
        occupationActuelle = Mathf.Max(0, occupationActuelle - 1);
    }

    public bool EstDisponible()
    {
        return occupationActuelle < capaciteMax;
    }
}
