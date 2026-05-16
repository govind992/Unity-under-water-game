using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UnderwaterEffect : MonoBehaviour
{
    [Header("Post Process Volumes")]
    public PostProcessVolume underwaterVolume;

    [Header("Fog Settings")]
    public Color underwaterFogColor = new Color(0.0f, 0.15f, 0.35f);
    public float underwaterFogDensity = 0.05f;

    private Color originalFogColor;
    private float originalFogDensity;
    private bool originalFogEnabled;

    void Start()
    {
        // Save original fog settings
        originalFogColor = RenderSettings.fogColor;
        originalFogDensity = RenderSettings.fogDensity;
        originalFogEnabled = RenderSettings.fog;

        underwaterVolume.weight = 0f; // Start disabled
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnterWater();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitWater();
        }
    }

    void EnterWater()
    {
        // Enable post processing
        underwaterVolume.weight = 1f;

        // Enable fog for depth effect
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = underwaterFogColor;
        RenderSettings.fogDensity = underwaterFogDensity;

        // Darken ambient light (dim underwater)
        RenderSettings.ambientIntensity = 0.3f;
    }

    void ExitWater()
    {
        underwaterVolume.weight = 0f;

        // Restore fog
        RenderSettings.fog = originalFogEnabled;
        RenderSettings.fogColor = originalFogColor;
        RenderSettings.fogDensity = originalFogDensity;
        RenderSettings.ambientIntensity = 1f;
    }
}