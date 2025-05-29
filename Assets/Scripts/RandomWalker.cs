using UnityEngine;

public class RandomWalker : MonoBehaviour
{
    public float moveRadius = 10f;
    public float waitTime = 3f;

    private float timer;
    private Vector3 origin;
    private PathfindingAI pathfinding;

    void Start()
    {
        pathfinding = GetComponent<PathfindingAI>();
        origin = transform.position;
        timer = waitTime;
    }

    void Update()
    {
        if (pathfinding.target == null && timer <= 0f)
        {
            Vector3 newPos = origin + new Vector3(Random.Range(-moveRadius, moveRadius), Random.Range(-moveRadius, moveRadius), 0);
            pathfinding.setTargetPos(newPos);
            timer = waitTime + Random.Range(0f, 2f); // variation
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
