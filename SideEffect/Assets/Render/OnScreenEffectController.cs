using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cyan;

public class OnScreenEffectController : MonoBehaviour
{
    [SerializeField] private UniversalRendererData rendererData = null;
    [SerializeField] private string featureName = null;

    private bool transitioning;
    private float startTime;

    private bool TryGetFeature(out ScriptableRendererFeature feature)
    {
        feature = rendererData
            .rendererFeatures
            .Where((f) => f.name == featureName)
            .FirstOrDefault();
        
        return feature != null;
    }

    public void AddMaterial(Material material)
    {
        if (TryGetFeature(out var feature))
        {
            Debug.Log(material);
            var blitFeature = feature as Blit;
            blitFeature.AddMaterial(material);
        }
    }

    public void RemoveMaterial(Material material)
    {
        if (TryGetFeature(out var feature))
        {
            var blitFeature = feature as Blit;
            blitFeature.RemoveMaterial(material);
        }
    }
}
