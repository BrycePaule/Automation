using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    [Header("World Settings")]
    public float Seed;
    public int MapSize;
	public float XOffset;
	public float YOffset;

    [Header("Noise Profiles")]
    public NoiseProfile Base;
    public NoiseProfile Gem1;
    public NoiseProfile Gem2;

    private MapToken[,] baseMap;
    private List<MapToken> baseTokens = new List<MapToken>();

    private MapToken[,] gem1Map;
    private List<MapToken> gem1Tokens = new List<MapToken>();

    private MapToken[,] gem2Map;
    private List<MapToken> gem2Tokens = new List<MapToken>();

    private List<MapToken[,]> mapsToBlend = new List<MapToken[,]>();

    [Header("References")]
    [SerializeField] private PerlinNoiseMapGenerator perlGen;
    [SerializeField] private scr_MapAsset MapAsset;

    private void Awake()
    {
        if (MapAsset == null)
        {
            MapAsset = GenerateNewMap();
        }
    }

    private void Start()
    {
        TilemapManager.Instance.SetTiles(MapAsset.Tokens, MapSize);
    }

    private scr_MapAsset GenerateNewMap()
    {
        // BASE
        baseTokens.Add(MapToken.Ground);
        baseTokens.Add(MapToken.AlternateGround);

        perlGen.SetValues(Seed, MapSize, XOffset, YOffset, Base.Scale, Base.NoiseValues, Base.HeightThresholds, Base.Colours);
        baseMap = perlGen.GenerateMap(baseTokens);

        // GEMS
        gem1Tokens.Add(MapToken.Empty);
        gem1Tokens.Add(MapToken.Gem1);

        perlGen.SetValues(Seed, MapSize, XOffset, YOffset, Gem1.Scale, Gem1.NoiseValues, Gem1.HeightThresholds, Gem1.Colours);
        gem1Map = perlGen.GenerateMap(gem1Tokens);

        gem2Tokens.Add(MapToken.Empty);
        gem2Tokens.Add(MapToken.Gem2);

        perlGen.SetValues(Seed, MapSize, XOffset, YOffset, Gem2.Scale, Gem2.NoiseValues, Gem2.HeightThresholds, Gem2.Colours);
        gem2Map = perlGen.GenerateMap(gem2Tokens);

        // BLENDING
        mapsToBlend.Add(baseMap);
        mapsToBlend.Add(gem1Map);
        mapsToBlend.Add(gem2Map);

		scr_MapAsset newMap = scr_MapAsset.CreateInstance<scr_MapAsset>();
        newMap.Tokens = Blend(mapsToBlend);

		string PATH = AssetDatabase.GenerateUniqueAssetPath("Assets/Project/ScriptableObjects/Maps/map_NEWMAP.asset");
		AssetDatabase.CreateAsset(newMap, PATH);

        return newMap;
    }

    private MapToken[,] Blend(List<MapToken[,]> mapsToAdd)
    {
        MapToken[,] result = new MapToken[MapSize, MapSize];

        foreach (var map in mapsToAdd)
        {
            for (int y = 0; y < MapSize; y++)
            {
                for (int x = 0; x < MapSize; x++)
                {
                    if (map[y, x] == MapToken.Empty) { continue; }
                    result[y, x] = map[y, x];
                }
            }
            
        }

        return result;
    }
}

