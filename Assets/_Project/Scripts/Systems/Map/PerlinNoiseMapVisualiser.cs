using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace bpdev
{
    [ExecuteInEditMode]
    public class PerlinNoiseMapVisualiser : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer sr;

        [Header("Asset")]
        public bool UseSettingsAsset;
        public PerlinSettings SelectedPerlinSettings;

        [Header("Settings")]
        [Range(1, 512)] public int MapSize;
        public float Seed;
        public int XOffset;
        public int YOffset;

        public float Scale;
        public List<NoiseFreqAmp> NoiseValues;
        public List<Threshold> HeightThresholds;

        [Header("Colours (for vis only)")]
        public bool UseManualColours;
        public List<Color> Colours;

        private PerlinNoiseMapGenerator perlinGen;

        private void Update()
        {
            if (UseSettingsAsset)
            {
                Seed = SelectedPerlinSettings.Seed;
                MapSize = SelectedPerlinSettings.MapSize;
                XOffset = SelectedPerlinSettings.XOffset;
                YOffset = SelectedPerlinSettings.YOffset;
                Scale = SelectedPerlinSettings.Scale;
                NoiseValues = SelectedPerlinSettings.NoiseValues;
                HeightThresholds = SelectedPerlinSettings.HeightThresholds;
            }

            transform.position = new Vector3(MapSize/2, MapSize/2, 0);
            transform.localScale = new Vector3(MapSize, MapSize, 1);

            BalanceThresholdAndColourNumbers();
            UpdateColours();
            sr.sharedMaterial.mainTexture = GenerateTexture();
        }

        //	Visualisation
        private Texture2D GenerateTexture()
        {
            Texture2D tex = new Texture2D(MapSize, MapSize);
            perlinGen = new PerlinNoiseMapGenerator(Seed, MapSize, XOffset, YOffset, Scale, NoiseValues, HeightThresholds, Colours);

            foreach (Vector3Int point in Utils.EvaluateGrid(MapSize))
            {
                float _height = perlinGen.GetHeight((Vector2Int) point);
                int _index = perlinGen.GetThresholdIndex(_height);
                tex.SetPixel(point.y, point.x, Colours[_index]);
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

        // Saving - called by editor
        public void SaveNewAsset()
        {
            PerlinSettings _settings = PerlinSettings.CreateInstance<PerlinSettings>();
            _settings.Seed = Seed;
            _settings.MapSize = MapSize;
            _settings.XOffset = XOffset;
            _settings.YOffset = YOffset;
            _settings.Scale = Scale;
            _settings.NoiseValues = NoiseValues;
            _settings.HeightThresholds = HeightThresholds;

            string _path = "Assets/Project/ScriptableObjects/PerlinSettings/New PerlinSetting.asset";
            AssetDatabase.CreateAsset(_settings, _path);

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = _settings;

            Debug.Log("Saved new PerlinSetting asset.");
            EditorUtility.SetDirty(_settings);
        }

        public void OverrideSelectedAsset()
        {
            SelectedPerlinSettings.Seed = Seed;
            SelectedPerlinSettings.MapSize = MapSize;

            SelectedPerlinSettings.Scale = Scale;
            SelectedPerlinSettings.YOffset = YOffset;
            SelectedPerlinSettings.XOffset = XOffset;

            SelectedPerlinSettings.NoiseValues = NoiseValues;
            SelectedPerlinSettings.HeightThresholds = HeightThresholds;

            Debug.Log($"Overrided {SelectedPerlinSettings.ToString()}.");
            EditorUtility.SetDirty(SelectedPerlinSettings);
        }

    }
}