using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("World Settings")]
    public float Seed;
    public int MapSize;
	public float XOffset;
	public float YOffset;

    [Header("Noise Profiles")]
    public NoiseProfile Base;
    // public NoiseSetting Biomes;
    public NoiseProfile Gem1;
    public NoiseProfile Gem2;

    [Header("Tiles")]
    public MyTile BaseTile;
    public MyTile AltBaseTile;
    public MyTile Gem1Tile;
    public MyTile Gem2Tile;

    [Header("References")]
    [SerializeField] private PerlinNoiseMapGenerator perlGen;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private Tilemap tilemap;

    private void Awake()
    {
        // BASE
        List<MapToken> baseTokens = new List<MapToken>();
        baseTokens.Add(MapToken.Ground);
        baseTokens.Add(MapToken.AlternateGround);

        perlGen.SetValues(Seed, MapSize, XOffset, YOffset, Base.Scale, Base.NoiseValues, Base.HeightThresholds, Base.Colours);
        MapToken[,] baseMap = perlGen.GenerateMap(baseTokens);

        // GEMS
        List<MapToken> gem1Tokens = new List<MapToken>();
        gem1Tokens.Add(MapToken.Empty);
        gem1Tokens.Add(MapToken.Gem1);

        List<MapToken> gem2Tokens = new List<MapToken>();
        gem2Tokens.Add(MapToken.Empty);
        gem2Tokens.Add(MapToken.Gem2);

        perlGen.SetValues(Seed, MapSize, XOffset, YOffset, Gem1.Scale, Gem1.NoiseValues, Gem1.HeightThresholds, Gem1.Colours);
        MapToken[,] gem1Map = perlGen.GenerateMap(gem1Tokens);

        perlGen.SetValues(Seed, MapSize, XOffset, YOffset, Gem2.Scale, Gem2.NoiseValues, Gem2.HeightThresholds, Gem2.Colours);
        MapToken[,] gem2Map = perlGen.GenerateMap(gem2Tokens);

        // BLENDING
        List<MapToken[,]> maps = new List<MapToken[,]>();
        maps.Add(baseMap);
        maps.Add(gem1Map);
        maps.Add(gem2Map);

        SetTiles(Blend(maps));
    }

    private void SetTiles(MapToken[,] map)
    {
        for (int y = 0; y < MapSize; y++)
        {
            for (int x = 0; x < MapSize; x++)
            {
                Vector3Int pos = new Vector3Int(y, x, 0);

                if (map[y, x] == MapToken.Ground) { tilemap.SetTile(pos, BaseTile); }
                if (map[y, x] == MapToken.AlternateGround) { tilemap.SetTile(pos, AltBaseTile); }
                if (map[y, x] == MapToken.Gem1) { tilemap.SetTile(pos, Gem1Tile); }
                if (map[y, x] == MapToken.Gem2) { tilemap.SetTile(pos, Gem2Tile); }
            }
        }
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

