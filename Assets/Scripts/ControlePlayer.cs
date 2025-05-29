using UnityEngine;

public class ControlePlayer : MonoBehaviour
{
    private float speed = 2.0f;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 newPosition = transform.position + new Vector3(horizontal, vertical, 0);

        animator.SetFloat("moveX", horizontal);
        animator.SetFloat("moveY", vertical);
        animator.SetBool("isMoving", true);

        if (isWalkable(horizontal, vertical))
        {
            transform.position = newPosition;
        }
    }

    private bool isWalkable(float x, float y)
    {
        if (Physics2D.OverlapCircle(transform.position + new Vector3(x, y, 0), 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }
}
