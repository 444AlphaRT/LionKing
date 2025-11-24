using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement speed is configured in the Inspector (not hard-coded in logic)
    [SerializeField] private float moveSpeed = 5f;

    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    // Stores the player's movement direction based on input
    private Vector2 movement = Vector2.zero;

    private void Awake()
    {
        // Cache the Rigidbody2D on this GameObject to avoid repeated lookups
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Read raw input from keyboard (WASD / Arrow keys)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Create a direction vector from input and normalize it
        movement = new Vector2(horizontalInput, verticalInput).normalized;
    }

    private void FixedUpdate()
    {
        // Apply velocity based on movement direction and configured speed
        rb.linearVelocity = movement * moveSpeed;
    }
}
