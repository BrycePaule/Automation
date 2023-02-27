using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PerlinNoiseMapGenerator : MonoBehaviour
{
    private float seed;
    private int mapSize;
    private float xOffset;
    private float yOffset;

    private float scale;
    private List<NoiseFreqAmp> noiseValues;
    private List<Threshold> heightThresholds;
    private List<Color> colours;

    public void SetValues(float _seed, int _mapSize, float _xOffset, float _yOffset, float _scale, List<NoiseFreqAmp> _noiseValues, List<Threshold> _heightThresholds, List<Color> _colours)
    {
        this.seed = _seed;
        this.mapSize = _mapSize;
        this.xOffset = _xOffset;
        this.yOffset = _yOffset;

        this.scale = _scale;
        this.noiseValues = _noiseValues;
        this.heightThresholds = _heightThresholds;
        this.colours = _colours;
    }

    private float PerlinSample(int x, int y, NoiseFreqAmp fa)
    {
        float xCoord = (float) x / mapSize * scale + xOffset;
        float yCoord = (float) y / mapSize * scale + yOffset;

        return Mathf.PerlinNoise((xCoord + mapSize * seed) * fa.Frequency, (yCoord + mapSize * seed) * fa.Frequency) * fa.Amplitude;
    }

    public float[,] GenerateHeightMap()
    {
        float[,] _heightMap = new float[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float _height = 0;
                foreach (NoiseFreqAmp freqAmp in noiseValues)
                {
                    _height += Mathf.Clamp01(PerlinSample(x, y, freqAmp));
                }

                _heightMap[y, x] = _height;
            }
        }

        return _heightMap;
    }

    public MapToken[,] GenerateMap(List<MapToken> tokens)
    {
        if (heightThresholds.Count > tokens.Count)
        {
            throw new System.Exception("Cannot generate map with more thresholds than tokens.");
        }

        float[,] heightMap = GenerateHeightMap();
        MapToken[,] map = new MapToken[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int index = GetThresholdIndex(heightThresholds, heightMap[y, x]);
                map[y, x] = tokens[index];
            }
        }

        return map;
    }

    private int GetThresholdIndex(List<Threshold> heightThresholds, float height)
    {
        int _index = 0;

        foreach (var threshold in heightThresholds)
        {
            if (height > threshold.Value) { _index += 1; }
        }

        return _index;
    }
}

