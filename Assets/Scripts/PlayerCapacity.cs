using UnityEngine;

public class PlayerCapacity : MonoBehaviour
{
    public int maxOccupants = 1;
    private int currentOccupants = 0;

    public bool VoirDisponibilite()
    {
        if (currentOccupants < maxOccupants)
        {
            currentOccupants++;
            return true;
        }
        return false;
    }

    public void Liberer()
    {
        currentOccupants = Mathf.Max(0, currentOccupants - 1);
    }
        public bool EstDisponible()
    {
        return currentOccupants < maxOccupants;
    }
}
