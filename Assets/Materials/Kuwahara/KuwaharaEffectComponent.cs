using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//from https://www.febucci.com/2022/05/custom-post-processing-in-urp/

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/CustomEffectComponent", typeof(UniversalRenderPipeline))]
public class KuwaharaEffectComponent : VolumeComponent, IPostProcessComponent
{
    // For example, an intensity parameter that goes from 0 to 1
    public ClampedIntParameter kernelSize = new ClampedIntParameter(value: 0, min: 0, max: 20, overrideState: true);
    // A color that is constant even when the weight changes
    // public NoInterpColorParameter overlayColor = new NoInterpColorParameter(Color.cyan);
    
    // Other 'Parameter' variables you might have
    
    // Tells when our effect should be rendered
    public bool IsActive() => kernelSize.value > 0;
   
    // I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}