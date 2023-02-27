using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public float Seed;
    public int MapSize;

	public float XOffset;
	public float YOffset;

    public NoiseSetting Base;
    // public NoiseSetting Biomes;
    public NoiseSetting Gems;

    public Tile BaseTile;
    public Tile AltBaseTile;
    public Tile GemTile;

    [Header("References")]
    [SerializeField] private PerlinNoiseMapGenerator perlinGen;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private Tilemap tilemap;

    private void Awake()
    {
        List<MapToken> baseTokens = new List<MapToken>();
        baseTokens.Add(MapToken.Ground);
        baseTokens.Add(MapToken.AlternateGround);

        MapToken[,] baseMap = perlinGen.GenerateMap(Base, baseTokens, Seed, MapSize, XOffset, YOffset);

        List<MapToken> gemTokens = new List<MapToken>();
        gemTokens.Add(MapToken.Empty);
        gemTokens.Add(MapToken.Gem);

        MapToken[,] gemMap = perlinGen.GenerateMap(Gems, gemTokens, Seed, MapSize, XOffset, YOffset);

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

                // if (map[y, x] == "0") { tilemap.SetTile(pos, BaseTile); }
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

