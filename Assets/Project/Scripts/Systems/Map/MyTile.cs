using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(menuName="Scriptables/Map/MyTile", fileName="New MyTile")]
public class MyTile : Tile
{

    [Header("Custom Settings")]
    public bool Blocked;
    public bool Mineable;

    public MyTile(bool blocked)
    {
        Blocked = blocked;
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }

	// public MyTile CreateInstance()
	// {
	// 	return Instantiate(this);
	// }
}