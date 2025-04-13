using UnityEngine;

// This is a utility class to help set up the Rage Effect prefab
// You can attach this to an empty GameObject and click the CreateRageEffectPrefab button
// in the Inspector to generate a RageEffect prefab with all necessary components
public class RageEffectPrefab : MonoBehaviour
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/Effects/Create Rage Effect Prefab")]
    public static void CreateRageEffectPrefab()
    {
        // Create parent game object
        GameObject rageEffectObj = new GameObject("RageEffect");
        
        // Add RageEffect component
        RageEffect rageEffect = rageEffectObj.AddComponent<RageEffect>();
        
        // Add audio source for sound effects
        AudioSource audioSource = rageEffectObj.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 20f;
        
        // Create particle system child
        GameObject particleObj = new GameObject("RageParticles");
        particleObj.transform.parent = rageEffectObj.transform;
        particleObj.transform.localPosition = Vector3.zero;
        
        // Add particle system
        ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();
        
        // Set up particle system main module
        var main = ps.main;
        main.duration = 10f; // Default rage duration
        main.loop = true;
        main.startLifetime = 2f;
        main.startSpeed = 1f;
        main.startSize = 0.2f;
        main.startColor = new Color(1f, 0.3f, 0.2f, 1f); // Reddish color
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 100;
        
        // Set up emission module
        var emission = ps.emission;
        emission.rateOverTime = 20f;
        
        // Set up shape module (shape around the character)
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 1f;
        shape.radiusThickness = 0f; // Emit from the surface
        
        // Set up renderer
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        
        // Assign particle system to RageEffect component
        rageEffect.rageParticles = ps;
        
        // Select the created object
        UnityEditor.Selection.activeGameObject = rageEffectObj;
        
        Debug.Log("Rage Effect Prefab created. Remember to save this as a prefab!");
    }
#endif
} 