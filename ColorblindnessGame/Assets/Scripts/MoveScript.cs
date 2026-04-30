using UnityEngine;
using UnityEngine.InputSystem;

public class MoveScript : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movementVector;
    SpriteRenderer spriteRenderer;
    Animator anim;

    public float speed = 1;
    public float jumpForce = 5;

    public bool canJump = false;
    public bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("VelocityX", movementVector.magnitude);

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
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            isJumping = true;
            anim.SetBool("Jumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            isJumping = false;
            anim.SetBool("Jumping", false);
        }
    }
}
