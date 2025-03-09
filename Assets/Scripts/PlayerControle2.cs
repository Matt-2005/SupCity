using UnityEngine;

public class randomNPC : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float[] moveTimeDurations = { 1f, 2f, 3f, 4f, 5f, 6f };
    [SerializeField] private float moveTime;
    [SerializeField] private float elapsedTime = 0f;


    private Vector2 currentDirection;
    private Vector3 startPosition;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        moveTime = moveTimeDurations[Random.Range(0, moveTimeDurations.Length)];
    }

    void Update()
    {
        moveRandomly();
    }

    void ChooseRandomDirection()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                currentDirection = Vector2.up;
                break;
            case 1:
                currentDirection = Vector2.down;
                break;
            case 2:
                currentDirection = Vector2.left;
                break;
            case 3:
                currentDirection = Vector2.right;
                break;
        }
    }

    void moveRandomly()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= moveTime)
        {
            elapsedTime = 0f;
            ChooseRandomDirection();
        }

        float horizontal = currentDirection.x * speed * Time.deltaTime;
        float vertical = currentDirection.y * speed * Time.deltaTime;

        animator.SetFloat("moveX", currentDirection.x);
        animator.SetFloat("moveY", currentDirection.y);
        animator.SetBool("isMoving", true);

        transform.Translate(new Vector3(horizontal, vertical, 0));
    }
}