using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 1.5f;
    Vector2 startPos;
    Vector2 targetPos;
    Vector2 direction;

    bool isMoving = false;
    bool movingToTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;

        switch (gameObject.tag)
        {
            case "PlatformUpDown":
                direction = Vector2.up;
                break;
            case "PlatformDownUp":
                direction = Vector2.down;
                break;
            case "PlatformLeftRight":
                direction = Vector2.left;
                break;
            case "PlatformRightLeft":
                direction = Vector2.right;
                break;
        }
        targetPos = startPos + direction * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) return;

        Vector2 destination = movingToTarget ? targetPos : startPos;

        transform.position = Vector2.MoveTowards(
            transform.position,
            destination,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, destination) < 0.01f)
        {
            transform.position = destination;
            isMoving = false;
        }
    }

    public void MovePlatform()
    {
        movingToTarget = true;
        isMoving = true;
    }

    public void ResetPlatform()
    {
        movingToTarget = false;
        isMoving = true;
    }
}
