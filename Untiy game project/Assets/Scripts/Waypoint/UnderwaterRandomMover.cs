using UnityEngine;

/// <summary>
/// Attach this script to your sphere GameObject.
/// Set the underwater boundary bounds in the Inspector to match your terrain.
/// The sphere will drift randomly within those bounds, staying fully submerged.
/// </summary>
public class UnderwaterRandomMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How fast the sphere moves toward its target position.")]
    public float moveSpeed = 2f;

    [Tooltip("How fast the sphere rotates while moving (visual effect).")]
    public float rotationSpeed = 30f;

    [Tooltip("Minimum time (seconds) before picking a new random target.")]
    public float minWanderInterval = 2f;

    [Tooltip("Maximum time (seconds) before picking a new random target.")]
    public float maxWanderInterval = 5f;

    [Header("Underwater Boundary (World Space)")]
    [Tooltip("Minimum corner of the allowed underwater volume (X, Y, Z).")]
    public Vector3 boundsMin = new Vector3(-20f, -15f, -20f);

    [Tooltip("Maximum corner of the allowed underwater volume (X, Y, Z).")]
    public Vector3 boundsMax = new Vector3(20f, -2f, 20f);

    [Header("Smoothing")]
    [Tooltip("Smoothing factor for direction changes (lower = smoother turns).")]
    [Range(0.5f, 10f)]
    public float steeringSmoothing = 3f;

    // Internal state
    private Vector3 _targetPosition;
    private Vector3 _currentVelocity;
    private float _wanderTimer;
    private float _nextWanderTime;

    private void Start()
    {
        // Validate that the sphere starts inside the bounds; clamp if not
        transform.position = ClampToBounds(transform.position);

        // Pick an initial random target
        PickNewTarget();
    }

    private void Update()
    {
        // Count down to next direction change
        _wanderTimer += Time.deltaTime;
        if (_wanderTimer >= _nextWanderTime)
        {
            PickNewTarget();
        }

        // Smoothly move toward the target using SmoothDamp for organic feel
        Vector3 desiredVelocity = (_targetPosition - transform.position).normalized * moveSpeed;
        _currentVelocity = Vector3.Lerp(_currentVelocity, desiredVelocity, steeringSmoothing * Time.deltaTime);

        Vector3 nextPosition = transform.position + _currentVelocity * Time.deltaTime;

        // Hard-clamp position so it never leaves the underwater volume
        nextPosition = ClampToBounds(nextPosition);

        // If we hit a wall, redirect toward the center to avoid getting stuck
        if (nextPosition != transform.position + _currentVelocity * Time.deltaTime)
        {
            PickNewTarget();
        }

        transform.position = nextPosition;

        // Rotate the sphere for a swimming-drift visual effect
        if (_currentVelocity.sqrMagnitude > 0.01f)
        {
            transform.Rotate(_currentVelocity.normalized * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Picks a new random position within the underwater bounds and resets the timer.
    /// </summary>
    private void PickNewTarget()
    {
        _targetPosition = new Vector3(
            Random.Range(boundsMin.x, boundsMax.x),
            Random.Range(boundsMin.y, boundsMax.y),
            Random.Range(boundsMin.z, boundsMax.z)
        );

        _wanderTimer = 0f;
        _nextWanderTime = Random.Range(minWanderInterval, maxWanderInterval);
    }

    /// <summary>
    /// Clamps a position so it stays within the defined underwater bounds.
    /// </summary>
    private Vector3 ClampToBounds(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, boundsMin.x, boundsMax.x);
        pos.y = Mathf.Clamp(pos.y, boundsMin.y, boundsMax.y);
        pos.z = Mathf.Clamp(pos.z, boundsMin.z, boundsMax.z);
        return pos;
    }

    /// <summary>
    /// Draws the underwater boundary as a yellow wireframe box in the Scene view.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.8f, 1f, 0.3f);
        Vector3 center = (boundsMin + boundsMax) / 2f;
        Vector3 size   = boundsMax - boundsMin;
        Gizmos.DrawCube(center, size);

        Gizmos.color = new Color(0f, 0.8f, 1f, 1f);
        Gizmos.DrawWireCube(center, size);

        // Draw the current target
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_targetPosition, 0.3f);
    }
}