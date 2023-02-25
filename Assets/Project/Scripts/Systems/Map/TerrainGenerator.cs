using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
	[Header("References")]
	public bool WriteTextureToSprite;
	public SpriteRenderer SR;

	[Header("Settings")]
	public bool UseInspectorSeed;
	public float Seed;
	public int MapSize;
	public float Scale;
	public float XOffset;
	public float YOffset;

	// [Header("Step Settings")]
	// public int steps;
	// public float stepDarkenPercent;

	[Header("Noise")]
	public List<NoiseFreqAmp> TerrainNoise;

	[Header("Heights")]
	public List<Threshold> HeightThresholds;

	[Header("Colours")]
	public List<Color> Colours;

	private void Awake()
	{
		if (!UseInspectorSeed) { Seed = Random.Range(0f, 999f); }
	}

	private void Update()
	{
		if (WriteTextureToSprite)
		{
			if (Colours?.Count != HeightThresholds?.Count + 1) { Debug.Log("Needs to be one less Threshold than Colour"); }
			float[,] terrainMap = GenerateHeightMap();
			float[,] biomeMap = GenerateHeightMap();
			SR.sharedMaterial.mainTexture = GenerateTextureBlend(terrainMap, biomeMap);
		}
	}

	// HeightMap
	public float[,] GenerateHeightMap()
	{
		float[,] heightMap = new float[MapSize, MapSize];

		for (int y = 0; y < MapSize; y++)
		{
			for (int x = 0; x < MapSize; x++)
			{
				float height = 0;
				foreach (NoiseFreqAmp FA in TerrainNoise)
				{
					height += Mathf.Clamp01(PerlinSample(x, y, FA.Frequency, FA.Amplitude));
				}

				heightMap[x, y] = height;
			}
		}

		return heightMap;
	}

	private float PerlinSample(int x, int y, float freq, float amp)
	{
		float xCoord = (float) x / MapSize * Scale + XOffset;
		float yCoord = (float) y / MapSize * Scale + YOffset;

		return Mathf.PerlinNoise((xCoord + MapSize * Seed) * freq, (yCoord + MapSize * Seed) * freq) * amp;
	}

	//	Textures
	public Texture2D GenerateTexture()
	{
		Texture2D tex = new Texture2D(MapSize, MapSize);
		float[,] heightMap = GenerateHeightMap();

		for (int y = 0; y < MapSize; y++)
		{
			for (int x = 0; x < MapSize; x++)
			{
				tex.SetPixel(x, y, GetTerrainColour(heightMap[x, y], 0));
			}
		}

		tex.Apply();
		return tex;
	}

	public Texture2D GenerateTextureBlend(float[,] terrainMap, float[,] biomeMap)
	{
		Texture2D tex = new Texture2D(MapSize, MapSize);

		for (int y = 0; y < MapSize; y++)
		{
			for (int x = 0; x < MapSize; x++)
			{
				tex.SetPixel(x, y, GetTerrainColour(terrainMap[x, y], biomeMap[x, y]));
			}
		}

		tex.Apply();
		return tex;
	}

	// Colours
	public Color GetTerrainColour(float terrainHeight, float biomeHeight)
	{
		for (int i = 0; i < HeightThresholds.Count; i++)
		{
			if (terrainHeight < HeightThresholds[0].Value) { return Colours[0];}
			if (terrainHeight >= HeightThresholds[HeightThresholds.Count-1].Value) { return Colours[HeightThresholds.Count]; }

			if (terrainHeight >= HeightThresholds[i].Value & terrainHeight < HeightThresholds[i+1].Value)
			{
				return Colours[i + 1];
			}
		}

		return Color.black;
	}

	private float[] GetBiomeColourSteps(int steps, float lower = 0f, float upper = 1.01f)
	{
		float[] biomeHeightSteps = new float[steps];

		for (int i = 0; i < steps; i++)
		{
			if (lower == 0f)
			{
				biomeHeightSteps[i] = (upper / (steps + 1)) * (i + 1);
			}
			else
			{
				biomeHeightSteps[i] = lower + (((upper / lower) / (steps + 1)) * (i + 1));
			}
		}

		return biomeHeightSteps;
	}
}

