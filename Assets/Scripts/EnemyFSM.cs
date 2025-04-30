using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum State { ChasePlayer, EscapeFromBomb, SearchPowerUp }

    [Header("Speeds")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float escapeSpeed = 4f;
    [SerializeField] private float searchSpeed = 2f;

    [Header("Detection Radii per State")]
    [SerializeField] private float chaseRadius = 5f;          // detect player
    [SerializeField] private float escapeRadius = 5f;         // detect bomb
    [SerializeField] private float searchRadius = 5f;         // detect powerUp

    [Header("Detection Tags & Layers")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string bombTag = "Bomb";
    [SerializeField] private string powerUpTag = "PowerUp";
    [SerializeField] private LayerMask detectionMask;

    private State? currentState = null; // Nullable to represent no active state
    private Rigidbody2D rb;
    private Vector2 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDir = Vector2.zero; // No movement initially
    }

    void Update()
    {
        // Priority transitions with state-specific radii
        if (DetectInRadius(bombTag, escapeRadius, out Vector2 bombDir))
        {
            Debug.Log("Bomb detected! Changing state to EscapeFromBomb.");
            ChangeState(State.EscapeFromBomb, -bombDir);
        }
        else if (DetectInRadius(playerTag, chaseRadius, out Vector2 playerDir))
        {
            Debug.Log("Player detected! Changing state to ChasePlayer.");
            ChangeState(State.ChasePlayer, playerDir);
        }
        else if (DetectInRadius(powerUpTag, searchRadius, out Vector2 puDir))
        {
            Debug.Log("PowerUp detected! Changing state to SearchPowerUp.");
            ChangeState(State.SearchPowerUp, puDir);
        }
        else
        {
            Debug.Log("No targets detected. Entering Sleep state.");
            Sleep(); // Call Sleep when no other state is active
        }
    }

    void FixedUpdate()
    {
        float speed = 0f; // Default speed
        if (currentState.HasValue)
        {
            switch (currentState.Value)
            {
                case State.ChasePlayer:
                    speed = chaseSpeed;
                    Debug.Log("Moving in ChasePlayer state.");
                    break;
                case State.EscapeFromBomb:
                    speed = escapeSpeed;
                    Debug.Log("Moving in EscapeFromBomb state.");
                    break;
                case State.SearchPowerUp:
                    speed = searchSpeed;
                    Debug.Log("Moving in SearchPowerUp state.");
                    break;
            }
        }
        rb.velocity = moveDir * speed; // Set velocity based on direction and speed
        Debug.Log($"Current Velocity: {rb.velocity}");
    }

    private void ChangeState(State newState, Vector2 dir)
    {
        currentState = newState;
        moveDir = dir.normalized;
        Debug.Log($"State changed to {newState}. Move direction: {moveDir}");
    }

    private void Sleep()
    {
        if (currentState != null)
        {
            Debug.Log("Entering Sleep State");
            currentState = null; // Clear the current state
            moveDir = Vector2.zero; // Stop movement
            rb.velocity = Vector2.zero; // Ensure velocity is also cleared
        }
    }

    // Radial detection
    private bool DetectInRadius(string tag, float radius, out Vector2 dir)
    {
        dir = Vector2.zero;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, detectionMask);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(tag))
            {
                dir = (hit.transform.position - transform.position).normalized;
                Debug.Log($"Detected {tag} within radius {radius}. Direction: {dir}");
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Vector3 p = transform.position;

        // ChasePlayer detection radius (green)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(p, chaseRadius);

        // EscapeFromBomb detection radius (red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(p, escapeRadius);

        // SearchPowerUp detection radius (yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(p, searchRadius);
    }
#endif
}
