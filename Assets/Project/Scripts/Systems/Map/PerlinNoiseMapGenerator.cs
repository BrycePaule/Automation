using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class PerlinNoiseMapGenerator : MonoBehaviour
{
    [Header("References")]
    public bool OverrideInspectorSettings;
    public NoiseSetting SelectedSettings;
    public SpriteRenderer SR;

    [Header("Settings")]
    public bool EnableVisualiser;

    public float Seed;
    public int MapSize;
    public float Scale;
    public float XOffset;
    public float YOffset;
    public List<NoiseFreqAmp> NoiseValues;
    public List<Threshold> HeightThresholds;

    [Header("Colours (for vis only)")]
    public bool UseManualColours;
    public List<Color> Colours;

    private void Update()
    {
        if (OverrideInspectorSettings)
        {
            Seed = SelectedSettings.Seed;
            MapSize = SelectedSettings.MapSize;

            Scale = SelectedSettings.Scale;
            YOffset = SelectedSettings.YOffset;
            XOffset = SelectedSettings.XOffset;

            NoiseValues = SelectedSettings.NoiseValues;
            HeightThresholds = SelectedSettings.HeightThresholds;
            Colours = SelectedSettings.Colours;
        }

        if (EnableVisualiser)
        {
            SR.enabled = true;;
        }
        else
        {
            SR.enabled = false;
        }

        BalanceThresholdAndColourNumbers();
        UpdateColours();
        SR.sharedMaterial.mainTexture = GenerateTexture();
    }

    // Generation
    private float PerlinSample(int x, int y, NoiseFreqAmp fa)
    {
        float xCoord = (float) x / MapSize * Scale + XOffset;
        float yCoord = (float) y / MapSize * Scale + YOffset;

        return Mathf.PerlinNoise((xCoord + MapSize * Seed) * fa.Frequency, (yCoord + MapSize * Seed) * fa.Frequency) * fa.Amplitude;
    }

    private float PerlinSample(int x, int y, NoiseFreqAmp fa, float seed, int mapSize, float scale, float xOffset, float yOffset)
    {
        float xCoord = (float) x / mapSize * scale + xOffset;
        float yCoord = (float) y / mapSize * scale + yOffset;

        return Mathf.PerlinNoise((xCoord + mapSize * seed) * fa.Frequency, (yCoord + mapSize * seed) * fa.Frequency) * fa.Amplitude;
    }

    public float[,] GenerateHeightMap()
    {
        float[,] _heightMap = new float[MapSize, MapSize];

        for (int y = 0; y < MapSize; y++)
        {
            for (int x = 0; x < MapSize; x++)
            {
                float _height = 0;
                foreach (NoiseFreqAmp _FA in NoiseValues)
                {
                    _height += Mathf.Clamp01(PerlinSample(x, y, _FA));
                }

                _heightMap[y, x] = _height;
            }
        }

        return _heightMap;
    }

    public float[,] GenerateHeightMap(NoiseSetting settings, float seed, int mapSize, float xOffset, float yOffset)
    {
        float[,] _heightMap = new float[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float _height = 0;
                foreach (NoiseFreqAmp _FA in settings.NoiseValues)
                {
                    _height += Mathf.Clamp01(PerlinSample(x, y, _FA, seed, mapSize, settings.Scale, xOffset, yOffset));
                }

                _heightMap[y, x] = _height;
            }
        }

        return _heightMap;
    }

    public MapToken[,] GenerateMap(NoiseSetting settings, List<MapToken> tokens, float seed, int mapSize, float xOffset, float yOffset)
    {
        if (settings.HeightThresholds.Count > tokens.Count)
        {
            throw new System.Exception("Cannot generate map with more thresholds than tokens.");
        }

        float[,] heightMap = GenerateHeightMap(settings, seed, mapSize, xOffset, yOffset);
        MapToken[,] map = new MapToken[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int index = GetThresholdIndex(settings, heightMap[y, x]);
                map[y, x] = tokens[index];
            }
        }

        return map;
    }

    private int GetThresholdIndex(NoiseSetting settings, float height)
    {
        int _index = 0;

        foreach (var threshold in settings.HeightThresholds)
        {
            if (height > threshold.Value) { _index += 1; }
        }

        return _index;
    }


    //	Visualisation
    public Texture2D GenerateTexture()
    {
        Texture2D tex = new Texture2D(MapSize, MapSize);
        float[,] heightMap = GenerateHeightMap();

        for (int y = 0; y < MapSize; y++)
        {
            for (int x = 0; x < MapSize; x++)
            {
                tex.SetPixel(x, y, GetTerrainColour(heightMap[y, x], 0));
            }
        }

        tex.Apply();
        return tex;
    }

    private void UpdateColours()
    {
        if (!UseManualColours)
        {
            Colours[0] = Color.white;
            Colours[Colours.Count - 1] = Color.black;
            float _mult = 1f / (Colours.Count - 1);

            for (int i = 1; i < Colours.Count - 1; i++)
            {
                float val = 1 - (1 * (_mult * i));
                Colours[i] = new Color(val, val, val);
            }
        }
    }

    private void BalanceThresholdAndColourNumbers()
    {
        // should be 1  - (as in 1 more colour than threshold)
        int _diff = Colours.Count - HeightThresholds.Count; 
        if (_diff == 1) { return; }

        if (_diff > 1)
        {
            int _numToRemove = _diff - 1;

            for (int i = 0; i < _numToRemove; i++)
            {
                Colours.RemoveAt(Colours.Count - i - 1);
            }
        }

        if (_diff < 1)
        {
            int _numToAdd = 1 - _diff;

            for (int i = 0; i < _numToAdd; i++)
            {
                Colours.Add(Color.black);                
            }
        }
    }

    public Color GetTerrainColour(float terrainHeight, float biomeHeight)
    {
        for (int i = 0; i < HeightThresholds.Count; i++)
        {
            if (terrainHeight < HeightThresholds[0].Value) { return Colours[0]; }
            if (terrainHeight >= HeightThresholds[HeightThresholds.Count-1].Value) { return Colours[HeightThresholds.Count]; }

            if (terrainHeight >= HeightThresholds[i].Value & terrainHeight < HeightThresholds[i+1].Value)
            {
                return Colours[i + 1];
            }
        }

        return Color.black;
    }

    // Saving - called by editor
    public void SaveNewAsset()
    {
        NoiseSetting _settings = NoiseSetting.CreateInstance<NoiseSetting>();

        _settings.Seed = Seed;
        _settings.MapSize = MapSize;

        _settings.Scale = Scale;
        _settings.YOffset = YOffset;
        _settings.XOffset = XOffset;

        _settings.NoiseValues = NoiseValues;
        _settings.HeightThresholds = HeightThresholds;
        _settings.Colours = Colours;

        string _path = "Assets/Project/ScriptableObjects/NoiseSettings/New NoiseSetting.asset";
        AssetDatabase.CreateAsset(_settings, _path);

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = _settings;

        Debug.Log("Saved new NoiseSetting asset.");
        EditorUtility.SetDirty(_settings);
    }

    public void OverrideSelectedAsset()
    {
        SelectedSettings.Seed = Seed;
        SelectedSettings.MapSize = MapSize;

        SelectedSettings.Scale = Scale;
        SelectedSettings.YOffset = YOffset;
        SelectedSettings.XOffset = XOffset;

        SelectedSettings.NoiseValues = NoiseValues;
        SelectedSettings.HeightThresholds = HeightThresholds;
        SelectedSettings.Colours = Colours;

        Debug.Log($"Overrided {SelectedSettings.ToString()}.");
        EditorUtility.SetDirty(SelectedSettings);
    }
}

