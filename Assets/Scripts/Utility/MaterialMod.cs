using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialMod
{
    
    //cached property indices
    public static readonly int PropColor = Shader.PropertyToID("_BaseColor");
    public static readonly int PropEmissive = Shader.PropertyToID("_EmissionColor");

    public static void SetOpacity(float opacity, MeshRenderer renderer, MaterialPropertyBlock propertyBlock)
    {
        SetOpacity(opacity, renderer, propertyBlock, PropColor);
    }
    
    public static void SetOpacity(float opacity, MeshRenderer renderer, MaterialPropertyBlock propertyBlock, int propColor)
    {
        renderer.GetPropertyBlock(propertyBlock);
        Color originalColor = propertyBlock.GetColor(propColor);
        Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, opacity);
        propertyBlock.SetColor(propColor, newColor);
        renderer.SetPropertyBlock(propertyBlock);
    }
    
    public static void SetEmissiveColor(Color newColor, MeshRenderer renderer, MaterialPropertyBlock propertyBlock)
    {
        SetEmissiveColor(newColor, renderer, propertyBlock, PropEmissive);
    }
    
    public static void SetEmissiveColor(Color newColor, MeshRenderer renderer, MaterialPropertyBlock propertyBlock, int propEmissive)
    {
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(propEmissive, newColor);
        renderer.SetPropertyBlock(propertyBlock);
    }
    
    public static Color GetEmissiveColor(MeshRenderer renderer, MaterialPropertyBlock propertyBlock)
    {
        return GetEmissiveColor(renderer, propertyBlock, PropEmissive);
    }

    public static Color GetEmissiveColor(MeshRenderer renderer, MaterialPropertyBlock propertyBlock, int propEmissive)
    {
        renderer.GetPropertyBlock(propertyBlock);
        return propertyBlock.GetColor(propEmissive);
    }

    public static IEnumerator LerpColor(Color end, float time, MeshRenderer renderer,
        MaterialPropertyBlock propertyBlock, int Prop)
    {
        float timeElapsed = 0f;
        renderer.GetPropertyBlock(propertyBlock);
        Color startColor = propertyBlock.GetColor(Prop);
        while (timeElapsed < time)
        {
            Color newColor = Color.Lerp(startColor, end, timeElapsed / time);
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(Prop, newColor);
            renderer.SetPropertyBlock(propertyBlock);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(Prop, end);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
