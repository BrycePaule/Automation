using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Map/Noise Settings", fileName = "New Noise Setting")]
public class NoiseSetting : ScriptableObject
{
    public float Seed;
    public int MapSize;
    public float Scale;
    public float XOffset;
    public float YOffset;
    public List<NoiseFreqAmp> NoiseValues;
    public List<Threshold> HeightThresholds;
    public List<Color> Colours;
}
