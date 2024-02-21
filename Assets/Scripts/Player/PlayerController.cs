using UnityEngine;
using UnityEngine.UIElements;

public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of player movement
    public float jumpForce = 10f; // Force applied when jumping
    public Transform groundCheck; // Reference to an empty GameObject to check if the player is grounded
    public LayerMask groundMask;
     Animator animator; // Reference to the Animator component

    private Rigidbody rb; // Reference to the player's Rigidbody component
    private bool isGrounded; // Flag to track if the player is grounded
    public static float HitPoints = 50f;
    bool isalive = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player
        animator=GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isalive) return;
        Isdead();
        // Check if the player is grounded using a sphere overlap
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
        if (!isGrounded) return;
        // Player movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        movement = Vector3.ClampMagnitude(movement, 1f); // Limit diagonal movement speed
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Player rotation (optional)
        if (movement != Vector3.zero && rb.velocity.y==0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
            animator.SetBool("IsRunning", true); // Set isRunning parameter in animator to true
        }
        else
        {
            animator.SetBool("IsRunning", false); // Set isRunning parameter in animator to false
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void Isdead()
    {
        if (HitPoints <= 0)
        {
            isalive = false;
            animator.SetTrigger("IsDead");
        }
    }
}
