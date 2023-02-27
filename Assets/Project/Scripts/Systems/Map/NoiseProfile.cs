using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Map/Noise Profile", fileName = "New Noise Profile")]
public class NoiseProfile : ScriptableObject
{
    public float Seed;
    public int MapSize;
    public float XOffset;
    public float YOffset;

    public float Scale;
    public List<NoiseFreqAmp> NoiseValues;
    public List<Threshold> HeightThresholds;
    public List<Color> Colours;

    public void SetValues(float seed, int mapSize, float xOffset, float yOffset, float scale, List<NoiseFreqAmp> noiseValues, List<Threshold> heightThresholds, List<Color> colours)
    {
        this.Seed = seed;
        this.MapSize = mapSize;
        this.XOffset = xOffset;
        this.YOffset = yOffset;

        this.Scale = scale;
        this.NoiseValues = noiseValues;
        this.HeightThresholds = heightThresholds;
        this.Colours = colours;
    }
}
