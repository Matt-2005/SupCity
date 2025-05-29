using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Gère les déplacements, le zoom et le drag de la caméra dans un environnement 2D basé sur une Tilemap.
/// Conserve la caméra dans les limites de la carte définies par la Tilemap "Background".
/// </summary>
public class CameraControl : MonoBehaviour
{
    /// <summary>Vitesse de déplacement clavier de la caméra.</summary>
    private float speed = 50.0f;

    /// <summary>Vitesse du zoom à la molette.</summary>
    private float zoomSpeed = 50.0f;

    /// <summary>Zoom minimum autorisé.</summary>
    private float minZoom = 3.0f;

    /// <summary>Zoom maximum autorisé.</summary>
    private float maxZoom = 70.0f;

    /// <summary>Vitesse de déplacement par glisser à la souris (clic droit).</summary>
    private float dragSpeed = 3.0f;

    /// <summary>Facteur d’accélération du mouvement (pour le Lerp).</summary>
    private float acceleration = 5.0f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private Vector3 velocity = Vector3.zero;
    private Vector3 dragOrigin;
    private float targetZoom;

    /// <summary>
    /// Initialise les limites de la carte en fonction de la Tilemap "Background"
    /// et définit le zoom initial.
    /// </summary>
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

    /// <summary>
    /// Met à jour le mouvement clavier, le drag souris et le zoom à chaque frame.
    /// </summary>
    void Update()
    {
        HandleKeyboardMovement();
        HandleMouseDrag();
        HandleZoom();
        ApplyMovement();
    }

    /// <summary>
    /// Gère les déplacements clavier (touches fléchées ou WASD).
    /// </summary>
    private void HandleKeyboardMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical, 0).normalized;
        velocity = Vector3.Lerp(velocity, direction * speed, Time.deltaTime * acceleration);
    }

    /// <summary>
    /// Gère le déplacement de la caméra en cliquant et glissant avec le bouton droit.
    /// </summary>
    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(dragOrigin) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            difference.z = 0f;

            transform.position += difference * dragSpeed;
            dragOrigin = Input.mousePosition;
        }
    }

    /// <summary>
    /// Gère le zoom avant/arrière avec la molette de la souris.
    /// Ajuste la position de la caméra pour garder le pointeur au même endroit.
    /// </summary>
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

    /// <summary>
    /// Applique le déplacement calculé tout en gardant la caméra dans les limites de la carte.
    /// </summary>
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
