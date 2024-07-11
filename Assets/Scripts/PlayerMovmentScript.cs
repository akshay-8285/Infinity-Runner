using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovmentScript : MonoBehaviour
{
    public float runSpeed = 5f;
    public float rotationSpeed = 100f;
    public float jumpForce = 7f;
    public float minX = -4f;
    public float maxX = 4f;
    public GameObject gameOverPanel; // Reference to Game Over panel
    public LayerMask groundLayer; // Layer for ground detection

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private bool isDead;
    private bool isRunning;

    private Vector3 startPosition; // Store initial position for reset

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isDead = false;
        isGrounded = true;
        isRunning = false; // Initially not running

        // Store initial position for reset
        startPosition = transform.position;

        // Freeze rotation to prevent unwanted rotation
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Enable Rigidbody interpolation for smoother movement
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Initially disable the game over panel
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (!isDead)
        {
            HandleInput();
        }

        // Check if the player is grounded
        CheckGroundStatus();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            HandleMovement();
        }
    }

    private void HandleInput()
    {
        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Ensure upward force applied
            isGrounded = false;
            animator.SetBool("jumping", true);
        }
    }

    private void HandleMovement()
    {
        if (isRunning)
        {
            // Move forward automatically
            Vector3 forwardMovement = transform.forward * runSpeed * Time.fixedDeltaTime;

            // Handle left/right movement
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 horizontalMovement = transform.right * horizontalInput * runSpeed * Time.fixedDeltaTime;

            Vector3 newPosition = rb.position + forwardMovement + horizontalMovement;

            // Clamp the horizontal position
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            rb.MovePosition(newPosition);
        }
        else
        {
            // Reset player to starting position if not running
            rb.MovePosition(startPosition);
        }
    }

    private void CheckGroundStatus()
    {
        // Use a small offset to detect the ground
        float groundCheckDistance = 0.1f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Reset the jumping animation when grounded
        if (isGrounded)
        {
            animator.SetBool("jumping", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("jumping", false);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset vertical velocity on landing
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Trigger death animation and game over panel
            if (!isDead)
            {
                isDead = true;
                animator.SetTrigger("death");
                StartCoroutine(ShowGameOverPanel());
            }
        }
    }

    private IEnumerator ShowGameOverPanel()
    {
        // Wait for the length of the death animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Get state info of base layer
        float deathAnimLength = stateInfo.length;
        yield return new WaitForSeconds(deathAnimLength);

        // Enable the game over panel
        gameOverPanel.SetActive(true);
    }

    public void StartRunning()
    {
        isRunning = true;
    }
}
