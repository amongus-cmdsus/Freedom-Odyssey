using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting pres",menuName ="Scriptable/Lighting preset,order =1")]
public class LightingPreset : ScriptableObject
{
   public Gradient AmbientColor;
   public Gradient DirectionalColor;

   public Gradient FogColor;
}
