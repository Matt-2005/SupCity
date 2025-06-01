using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip placementSound;
    public AudioSource audioSource; // à assigner dans l’inspecteur

    public GameObject prefab; // objet à poser

    public void PlacerObjet(Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);

        if (placementSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(placementSound);
        }
    }
}

