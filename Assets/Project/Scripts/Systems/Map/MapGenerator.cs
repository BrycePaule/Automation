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
    public NoiseProfile Gems;

    [Header("Tiles")]
    public MyTile BaseTile;
    public MyTile AltBaseTile;
    public MyTile GemTile;

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
        List<MapToken> gemTokens = new List<MapToken>();
        gemTokens.Add(MapToken.Empty);
        gemTokens.Add(MapToken.Gem);

        perlGen.SetValues(Seed, MapSize, XOffset, YOffset, Gems.Scale, Gems.NoiseValues, Gems.HeightThresholds, Gems.Colours);
        MapToken[,] gemMap = perlGen.GenerateMap(gemTokens);

        // BLENDING
        List<MapToken[,]> maps = new List<MapToken[,]>();
        maps.Add(baseMap);
        maps.Add(gemMap);

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
                if (map[y, x] == MapToken.Gem) { tilemap.SetTile(pos, GemTile); }
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

