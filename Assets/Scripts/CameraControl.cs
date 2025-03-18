using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraControl : MonoBehaviour
{
    private float speed = 50.0f;
    private float zoomSpeed = 50.0f;
    private float minZoom = 3.0f;
    private float maxZoom = 70.0f;
    private float dragSpeed = 3.0f;
    private float acceleration = 5.0f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private Vector3 velocity = Vector3.zero;
    private Vector3 dragOrigin;
    private float targetZoom;

    void Start()
    {
        Tilemap tilemap = GameObject.Find("Background").GetComponent<Tilemap>();

        Bounds tilemapBounds = tilemap.localBounds;

        minX = tilemapBounds.min.x;
        maxX = tilemapBounds.max.x;
        minY = tilemapBounds.min.y;
        maxY = tilemapBounds.max.y;

        targetZoom = Camera.main.orthographicSize;
    }

    void Update()
    {
        HandleKeyboardMovement();
        HandleMouseDrag();
        HandleZoom();
        ApplyMovement();
    }

    private void HandleKeyboardMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontal, vertical, 0).normalized;
        velocity = Vector3.Lerp(velocity, direction * speed, Time.deltaTime * acceleration);
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = MousePosition.MouseWorldPosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - MousePosition.MouseWorldPosition;
            transform.position += difference * dragSpeed;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            float zoomFactor = 1.2f;
            targetZoom *= (scroll > 0) ? 1 / zoomFactor : zoomFactor;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

            Vector3 mouseToCamera = MousePosition.MouseWorldPosition - transform.position;
            transform.position += mouseToCamera * (1 - targetZoom / Camera.main.orthographicSize);
        }
        
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * 5);
    }

    private void ApplyMovement()
    {
        Vector3 newPosition = transform.position + velocity * Time.deltaTime;
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        newPosition.x = Mathf.Clamp(newPosition.x, minX + cameraWidth / 2, maxX - cameraWidth / 2);
        newPosition.y = Mathf.Clamp(newPosition.y, minY + cameraHeight / 2, maxY - cameraHeight / 2);

        transform.position = newPosition;
    }
}
