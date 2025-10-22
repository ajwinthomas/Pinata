using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlatformVelocity : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private float distance = 5f;   // How far it moves from start
    [SerializeField] private float speed = 2f;      // Movement speed
    [SerializeField] private LayerMask passengerMask; // Which layers can ride (e.g. Player)

    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 previousPos;
    private Vector2 platformVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Kinematic so we control motion
        startPos = rb.position;
        previousPos = startPos;
    }

    private void FixedUpdate()
    {
        // 1. Move platform with PingPong
        float newX = startPos.x + distance * Mathf.PingPong(Time.time * speed, 1);
        Vector2 newPos = new Vector2(newX, rb.position.y);
        rb.MovePosition(newPos);

        // 2. Calculate velocity (units/second)
        platformVelocity = (newPos - previousPos) / Time.fixedDeltaTime;

        // 3. Move passengers riding the platform
        MovePassengers();

        // 4. Save position for next frame
        previousPos = newPos;
    }

    private void MovePassengers()
    {
        // Cast a box just above the platform to detect players standing on it
        Vector2 boxSize = rb.GetComponent<BoxCollider2D>().size;
        Vector2 boxCenter = rb.position + Vector2.up * 0.05f; // slightly above

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, passengerMask);

        foreach (Collider2D hit in hits)
        {
            Rigidbody2D passengerRb = hit.attachedRigidbody;
            if (passengerRb != null)
            {
                // Apply platform velocity to passenger
                passengerRb.position += platformVelocity * Time.fixedDeltaTime;
            }
        }
    }

    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 start = Application.isPlaying ? (Vector3)startPos : transform.position;
        Vector3 end = start + Vector3.right * distance;
        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.red;
        if (rb != null)
        {
            Vector2 boxSize = rb.GetComponent<BoxCollider2D>().size;
            Vector2 boxCenter = rb.position + Vector2.up * 0.05f;
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }
    }
}
