using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private float distance = 5f; // How far the platform moves from start
    [SerializeField] private float speed = 2f;    // How fast it moves

    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 previousPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Kinematic to control via MovePosition
        startPos = rb.position;
        previousPos = startPos;
    }

    private void FixedUpdate()
    {
        // 1. Calculate new X position using PingPong
        float newX = startPos.x + distance * Mathf.PingPong(Time.time * speed, 1);

        // 2. New platform position
        Vector2 newPosition = new Vector2(newX, rb.position.y);

        // 3. Move the platform via Rigidbody2D.MovePosition (physics-friendly)
        rb.MovePosition(newPosition);

        // 4. Calculate platform velocity (for moving the player)
        Vector2 platformVelocity = (newPosition - previousPos) / Time.fixedDeltaTime;

        // 5. Move any players standing on the platform
        MovePlayersOnPlatform(platformVelocity);

        // 6. Save position for next frame
        previousPos = newPosition;
    }

    // Optional: Detect players on platform using a trigger or collision
    private void MovePlayersOnPlatform(Vector2 platformVelocity)
    {
        // Raycast down from platform to detect standing players
        RaycastHit2D[] hits = Physics2D.BoxCastAll(rb.position, new Vector2(rb.GetComponent<BoxCollider2D>().size.x, 0.1f), 0f, Vector2.down, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Rigidbody2D playerRb = hit.collider.attachedRigidbody;
                if (playerRb != null)
                {
                    // Add the platform's velocity to the player's velocity
                    playerRb.position += platformVelocity * Time.fixedDeltaTime;
                }
            }
        }
    }

    // Optional: Draw gizmos to visualize platform movement
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.right * distance;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireCube(start, new Vector3(GetComponent<BoxCollider2D>().size.x, GetComponent<BoxCollider2D>().size.y, 0.1f));
        Gizmos.DrawWireCube(end, new Vector3(GetComponent<BoxCollider2D>().size.x, GetComponent<BoxCollider2D>().size.y, 0.1f));
    }
}
