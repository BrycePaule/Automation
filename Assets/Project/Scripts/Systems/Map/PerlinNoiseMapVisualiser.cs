using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PerlinNoiseMapVisualiser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PerlinNoiseMapGenerator perlinGen;
    [SerializeField] private SpriteRenderer sr;

    [Header("Settings")]
    public bool OverrideInspectorSettings;
    public NoiseProfile SelectedNoiseProfile;
    public float Seed;
    public int MapSize;
    public float XOffset;
    public float YOffset;

    public float Scale;
    public List<NoiseFreqAmp> NoiseValues;
    public List<Threshold> HeightThresholds;

    [Header("Colours (for vis only)")]
    public bool UseManualColours;
    public List<Color> Colours;

    private void Update()
    {
        if (OverrideInspectorSettings)
        {
            Seed = SelectedNoiseProfile.Seed;
            MapSize = SelectedNoiseProfile.MapSize;
            XOffset = SelectedNoiseProfile.XOffset;
            YOffset = SelectedNoiseProfile.YOffset;
            Scale = SelectedNoiseProfile.Scale;
            NoiseValues = SelectedNoiseProfile.NoiseValues;
            HeightThresholds = SelectedNoiseProfile.HeightThresholds;
            Colours = SelectedNoiseProfile.Colours;
        }

        perlinGen.SetValues(
            _seed: Seed,
            _mapSize: MapSize,
            _xOffset: XOffset,
            _yOffset: YOffset,
            _scale: Scale,
            _noiseValues: NoiseValues,
            _heightThresholds: HeightThresholds,
            _colours: Colours
        );

        BalanceThresholdAndColourNumbers();
        UpdateColours();
        sr.sharedMaterial.mainTexture = GenerateTexture();
    }

    //	Visualisation
    private Texture2D GenerateTexture()
    {
        Texture2D tex = new Texture2D(MapSize, MapSize);
        float[,] heightMap = perlinGen.GenerateHeightMap();

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

    private Color GetTerrainColour(float terrainHeight, float biomeHeight)
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

        return Color.yellow;
    }

    // Saving - called by editor
    public void SaveNewAsset()
    {
        NoiseProfile _profile = NoiseProfile.CreateInstance<NoiseProfile>();
        _profile.SetValues(Seed, MapSize, Scale, XOffset, YOffset, NoiseValues, HeightThresholds, Colours);

        string _path = "Assets/Project/ScriptableObjects/NoiseSettings/New NoiseSetting.asset";
        AssetDatabase.CreateAsset(_profile, _path);

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = _profile;

        Debug.Log("Saved new NoiseSetting asset.");
        EditorUtility.SetDirty(_profile);
    }

    public void OverrideSelectedAsset()
    {
        SelectedNoiseProfile.Seed = Seed;
        SelectedNoiseProfile.MapSize = MapSize;

        SelectedNoiseProfile.Scale = Scale;
        SelectedNoiseProfile.YOffset = YOffset;
        SelectedNoiseProfile.XOffset = XOffset;

        SelectedNoiseProfile.NoiseValues = NoiseValues;
        SelectedNoiseProfile.HeightThresholds = HeightThresholds;
        SelectedNoiseProfile.Colours = Colours;

        Debug.Log($"Overrided {SelectedNoiseProfile.ToString()}.");
        EditorUtility.SetDirty(SelectedNoiseProfile);
    }

}
