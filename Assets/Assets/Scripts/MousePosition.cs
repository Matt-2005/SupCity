using UnityEngine;

public class MousePosition : MonoBehaviour
{
    public static Vector3 MouseWorldPosition { get; private set; }

    void Update()
    {
        Vector3 tempPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPosition.z = 0f;
        MouseWorldPosition = tempPosition;
    }
}
