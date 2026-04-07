using UnityEngine;
using UnityEngine.InputSystem;

public class MoveScript : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movementVector;
    SpriteRenderer spriteRenderer;

    public float speed = 1;
    public float jumpForce = 5;

    public bool canJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(movementVector.magnitude > .2f)
        {
            rb.AddForce(new Vector2(movementVector.x * speed * Time.deltaTime, 0), ForceMode2D.Impulse); // Move Player
        }

        if (rb.linearVelocity.x < -0.2)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb.linearVelocity.x > 0.2)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnMove(InputValue input)
    {
        movementVector = input.Get<Vector2>();
    }

    public void OnJump(InputValue input)
    {
        if (canJump)
        {
            rb.AddForce(Vector2.up * jumpForce);
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}
