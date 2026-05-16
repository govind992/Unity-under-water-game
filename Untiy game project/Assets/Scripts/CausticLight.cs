using UnityEngine;

public class CausticLight : MonoBehaviour
{
    public float speed = 0.5f;
    public float intensityMin = 0.3f;
    public float intensityMax = 0.8f;

    private Light causticLight;

    void Start() => causticLight = GetComponent<Light>();

    void Update()
    {
        // Flicker intensity to simulate light through water
        causticLight.intensity = Mathf.Lerp(intensityMin, intensityMax,
            Mathf.PerlinNoise(Time.time * speed, 0f));

        // Slowly rotate to animate the caustic pattern
        transform.Rotate(0, 0, Mathf.Sin(Time.time * 0.3f) * 0.5f);
    }
}