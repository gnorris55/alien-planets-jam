using UnityEngine;

public class PlanetVisuals : MonoBehaviour
{
    [SerializeField] private Planet planet;
    [SerializeField] private Color planetColor;
    [SerializeField] private Transform miniMapVisual;
    [SerializeField] private ParticleSystem planetHitParticleEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        float planetRadius = planet.GetPlanetRadius();
        transform.localScale *= planetRadius;
        miniMapVisual.localScale *= planetRadius;

        Material planetShaderMaterial = GetComponent<SpriteRenderer>().material;

        planetShaderMaterial.SetColor("_PlanetColor", planetColor);
        
        float planetPixelSize = planetShaderMaterial.GetFloat("_PixelSize");
        float adjustedPixelSize = planetPixelSize * planetRadius;

        planetShaderMaterial.SetFloat("_PixelSize", adjustedPixelSize);
        planetShaderMaterial.SetVector("_UVOffset", new Vector2(Random.Range(0, 11), Random.Range(0, 11)));

         
        
    }

    public void CreatePlanetHitEffect(Vector3 location, float hitAngle)
    {
        var colorModule = planetHitParticleEffect.colorOverLifetime;
        colorModule.enabled = true;


        Gradient grad = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(planetColor, 0.0f); // At start
        colorKeys[1] = new GradientColorKey(Color.black, 1.0f);   // At end

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f); // Fully opaque at start
        alphaKeys[1] = new GradientAlphaKey(0.0f, 1.0f); // Fades out at end

        grad.SetKeys(colorKeys, alphaKeys);

        colorModule.color = grad;

        Instantiate(planetHitParticleEffect, location, Quaternion.Euler(0, 0, hitAngle));
    }

}
