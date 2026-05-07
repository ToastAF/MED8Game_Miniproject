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
    public float groundRayLength = 1.25f;

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
        //Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);

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

    bool GetRaycast()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundRayLength, LayerMask.GetMask("Ground"));
    }

    public void OnMove(InputValue input)
    {
        movementVector = input.Get<Vector2>();
    }

    public void OnJump(InputValue input)
    {
        if (GetRaycast())
        { 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            isJumping = true;
            anim.SetBool("Jumping", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("Jumping", false);
    }
}
