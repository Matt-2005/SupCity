using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraControl : MonoBehaviour
{
    private float speed = 50.0f;
    private float zoomSpeed = 50.0f;
    private float minZoom = 3.0f;
    private float maxZoom = 70.0f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    void Start()
    {
        Tilemap tilemap = GameObject.Find("Background").GetComponent<Tilemap>();

        Bounds tilemapBounds = tilemap.localBounds;

        minX = tilemapBounds.min.x;
        maxX = tilemapBounds.max.x;
        minY = tilemapBounds.min.y;
        maxY = tilemapBounds.max.y;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical")* speed * Time.deltaTime;

        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Vector3 newPosition = transform.position + new Vector3(horizontal, vertical, 0);

        newPosition.x = Mathf.Clamp(newPosition.x, minX + cameraWidth / 2, maxX - cameraWidth / 2);
        newPosition.y = Mathf.Clamp(newPosition.y, minY + cameraHeight / 2, maxY - cameraHeight / 2);

        transform.position = newPosition;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            float newSize = Camera.main.orthographicSize - scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }
}
