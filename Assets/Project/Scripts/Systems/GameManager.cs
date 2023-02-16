using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    
    [Range(2, 50)] public int size;

    [Header("References")]
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tile _tile;

    private void Start()
    {
        SetTiles();
    }

    private void SetTiles()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                _tilemap.SetTile(pos, _tile);
            }
        }
    }

}