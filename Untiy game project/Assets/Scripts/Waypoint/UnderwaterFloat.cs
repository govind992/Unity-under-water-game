using UnityEngine;

public class UnderwaterFloat : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float floatSpeed = 1f;
    public float floatHeight = 0.5f;

    [Header("Area Limits")]
    public Terrain terrain;
    public float waterLevel = 10f;   // Top underwater limit
    public float minHeightAboveTerrain = 1f;

    [Header("Random Range")]
    public float moveRange = 20f;

    private Vector3 targetPos;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        SetNewTarget();
    }

    void Update()
    {
        // Floating effect
        float floatY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Move smoothly
        Vector3 desiredPos = Vector3.Lerp(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // Terrain height check
        float terrainHeight = terrain.SampleHeight(desiredPos) + terrain.transform.position.y;

        // Keep inside underwater area
        desiredPos.y = Mathf.Clamp(
            terrainHeight + minHeightAboveTerrain + floatY,
            terrainHeight + minHeightAboveTerrain,
            waterLevel
        );

        transform.position = desiredPos;

        // New random target
        if (Vector3.Distance(transform.position, targetPos) < 1f)
        {
            SetNewTarget();
        }
    }

    void SetNewTarget()
    {
        TerrainData terrainData = terrain.terrainData;

        Vector3 terrainPos = terrain.transform.position;

        float randomX = Random.Range(
            terrainPos.x,
            terrainPos.x + terrainData.size.x
        );

        float randomZ = Random.Range(
            terrainPos.z,
            terrainPos.z + terrainData.size.z
        );

        float terrainHeight = terrain.SampleHeight(
            new Vector3(randomX, 0, randomZ)
        ) + terrainPos.y;

        float randomY = Random.Range(
            terrainHeight + minHeightAboveTerrain,
            waterLevel
        );

        targetPos = new Vector3(randomX, randomY, randomZ);
    }
}