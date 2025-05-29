using UnityEngine;

public class AutoDestroyAfterUse : MonoBehaviour
{

    public float delay = 2f;

    public void DéclencherDestruction()
    {
        Destroy(gameObject, delay);
    }
}
