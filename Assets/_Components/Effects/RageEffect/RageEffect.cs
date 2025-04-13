using System.Collections;
using UnityEngine;

public class RageEffect : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] public ParticleSystem rageParticles;
    [SerializeField] private Color rageEmissionColor = Color.red;
    [SerializeField] private float rageEmissionIntensity = 2f;
    [SerializeField] private float pulsateSpeed = 2f;
    [SerializeField] private float pulsateIntensity = 0.5f;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip rageActivationSound;
    [SerializeField] private AudioClip rageDeactivationSound;
    [SerializeField] private AudioSource audioSource;
    
    // Cache for material properties
    private Material[] characterMaterials;
    private Color[] originalEmissionColors;
    private float[] originalEmissionIntensities;
    
    private void Start()
    {
        // Get reference to the character's renderer
        Renderer characterRenderer = transform.parent.GetComponentInChildren<SkinnedMeshRenderer>();
        
        if (characterRenderer != null)
        {
            // Cache all materials
            characterMaterials = characterRenderer.materials;
            originalEmissionColors = new Color[characterMaterials.Length];
            originalEmissionIntensities = new float[characterMaterials.Length];
            
            // Store original emission values
            for (int i = 0; i < characterMaterials.Length; i++)
            {
                if (characterMaterials[i].HasProperty("_EmissionColor"))
                {
                    originalEmissionColors[i] = characterMaterials[i].GetColor("_EmissionColor");
                    originalEmissionIntensities[i] = originalEmissionColors[i].maxColorComponent;
                    
                    // Enable emission on the material
                    characterMaterials[i].EnableKeyword("_EMISSION");
                }
            }
        }
        
        // Play particles if available
        if (rageParticles != null)
        {
            rageParticles.Play();
        }
        
        // Play activation sound if available
        if (audioSource != null && rageActivationSound != null)
        {
            audioSource.PlayOneShot(rageActivationSound);
        }
        
        // Start the emission pulsation effect
        StartCoroutine(PulsateEmission());
    }
    
    private IEnumerator PulsateEmission()
    {
        float startTime = Time.time;
        
        while (true)
        {
            // Calculate pulsation based on sine wave
            float pulse = Mathf.Sin((Time.time - startTime) * pulsateSpeed) * pulsateIntensity + 1.0f;
            
            // Update emission on all materials
            for (int i = 0; i < characterMaterials.Length; i++)
            {
                if (characterMaterials[i].HasProperty("_EmissionColor"))
                {
                    characterMaterials[i].SetColor("_EmissionColor", rageEmissionColor * rageEmissionIntensity * pulse);
                }
            }
            
            yield return null;
        }
    }
    
    private void OnDestroy()
    {
        // Stop all coroutines
        StopAllCoroutines();
        
        // Reset material emission to original values
        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i] != null && characterMaterials[i].HasProperty("_EmissionColor"))
            {
                characterMaterials[i].SetColor("_EmissionColor", originalEmissionColors[i]);
                
                // If original didn't use emission, disable it
                if (originalEmissionIntensities[i] <= 0.01f)
                {
                    characterMaterials[i].DisableKeyword("_EMISSION");
                }
            }
        }
        
        // Play deactivation sound if available
        if (audioSource != null && rageDeactivationSound != null)
        {
            audioSource.PlayOneShot(rageDeactivationSound);
        }
    }
} 