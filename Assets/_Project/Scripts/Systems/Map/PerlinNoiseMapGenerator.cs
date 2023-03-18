using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public class PerlinNoiseMapGenerator
    {
        private float seed;
        private int mapSize;
        private float xOffset;
        private float yOffset;

        private float scale;
        private List<NoiseFreqAmp> noiseValues;
        private List<Threshold> heightThresholds;
        private List<Color> colours;

        public PerlinNoiseMapGenerator(float _seed, int _mapSize, float _xOffset, float _yOffset, float _scale, List<NoiseFreqAmp> _noiseValues, List<Threshold> _heightThresholds, List<Color> _colours)
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

        public PerlinNoiseMapGenerator(PerlinSettings settings)
        {
            this.seed = settings.Seed;
            this.mapSize = settings.MapSize;
            this.xOffset = settings.XOffset;
            this.yOffset = settings.YOffset;
            this.scale = settings.Scale;

            this.noiseValues = settings.NoiseValues;
            this.heightThresholds = settings.HeightThresholds;
        }

        private float ClampedPerlinSample(int x, int y, NoiseFreqAmp fa)
        {
            float scaledXOffset = xOffset / mapSize * scale;
            float scaledYOffset = yOffset / mapSize * scale;

            float xCoord = (float) x / mapSize * scale + scaledXOffset;
            float yCoord = (float) y / mapSize * scale + scaledYOffset;

            return Mathf.Clamp01(
                Mathf.PerlinNoise(
                    ((xCoord + mapSize * seed) * fa.Frequency) * fa.Amplitude,
                    ((yCoord + mapSize * seed) * fa.Frequency) * fa.Amplitude)
                );
        }

        public int GetThresholdIndex(float height)
        {
            int _index = 0;

            foreach (var threshold in heightThresholds)
            {
                if (height > threshold.Value) { _index += 1; }
            }

            return _index;
        }

        public float GetHeight(Vector2Int pos)
        {
            float _height = 0;

            foreach (var freqAmp in noiseValues)
            {
                _height += ClampedPerlinSample(pos.x, pos.y, freqAmp);
            }

            return _height;
        }

        public MapToken GetTokenAt(Vector3Int pos, List<MapToken> tokens)
        {
            float _height = GetHeight((Vector2Int) pos);
            int _index = GetThresholdIndex(_height);

            return tokens[_index];
        }

        public void Offset(Vector3Int playerOffset)
        {
            xOffset = playerOffset.x;
            yOffset = playerOffset.y;
        }

        // GENERATE FULL MAPS
        // old technique that is no longer used

        public float[,] GenerateHeightMap()
        {
            float[,] _heightMap = new float[mapSize, mapSize];

            foreach (Vector3Int point in Utils.EvaluateGrid(mapSize))
            {
                _heightMap[point.y, point.x] = GetHeight((Vector2Int) point);
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

            foreach (var point in Utils.EvaluateGrid(mapSize))
            {
                int index = GetThresholdIndex(heightMap[point.y, point.x]);
                map[point.y, point.x] = tokens[index];
            }

            return map;
        }


    }

}