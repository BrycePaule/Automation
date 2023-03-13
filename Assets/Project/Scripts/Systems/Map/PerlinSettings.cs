using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptables/Map/Perlin Settings", fileName = "New Perlin Settings")]
public class PerlinSettings : ScriptableObject
{
    [Header("Settings")]
    public float Seed;
    public int MapSize;
    public int XOffset;
    public int YOffset;
    public float Scale;

    [Header("Sliders")]
    public List<NoiseFreqAmp> NoiseValues;
    public List<Threshold> HeightThresholds;

    public List<MapToken> Tokens;
}
