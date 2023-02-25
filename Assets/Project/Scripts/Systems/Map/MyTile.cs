using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(menuName="Map/OffGrid Tile", fileName="New Tile")]
public class MyTile : TileBase
{

	// [Header("Custom Settings")]
	// [Space(20)]
	// public GroundType GroundType;
	// public TravelType TravelType;
	// public TileType TileType;

    // ----------------------------------

	// public List<TWeight<Sprite>> Sprites;
	// public List<GameObject> Objects;
	// public float timeSinceUpdated;
	// public bool Occupied => Objects.Count > 0;


	// public OffGridTile(GroundType _groundType, TileType _tileType, TravelType _travelType, List<TWeight<Sprite>> _sprites, List<GameObject> _objects)
	// {
	// 	GroundType = _groundType;
	// 	TravelType = _travelType;
	// 	Sprites = _sprites;
	// 	Objects = _objects;
	// }

	// public OffGridTile(GroundType _groundType, TileType _tileType, TravelType _travelType, Sprite _sprite, List<GameObject> _objects)
	// {
	// 	GroundType = _groundType;
	// 	TravelType = _travelType;
	// 	Sprites = new List<TWeight<Sprite>>(){ new TWeight<Sprite>(_sprite, 1) };
	// 	Objects = _objects;
	// }

	// public OffGridTile(GroundType _groundType, TileType _tileType, TravelType _travelType, List<TWeight<Sprite>> _sprites, GameObject _object)
	// {
	// 	GroundType = _groundType;
	// 	TravelType = _travelType;
	// 	Sprites = _sprites;
	// 	Objects = new List<GameObject>(){_object};;
	// }

	public MyTile CreateInstance()
	{
		MyTile tile = Instantiate(this);

		// if (spriteVariant == -1)
		// {
		// 	tile.sprite = OffGridUtils.TWeightRoll<Sprite>(Sprites);
		// }
		// else
		// {
		// 	Sprite[] _justSprites = Sprites.Select(item => item.Item).ToArray();
		// 	tile.sprite =  _justSprites[Mathf.Clamp(spriteVariant, 0, Sprites.Count - 1)];
		// }
		
		// tile.color = OffGridUtils.Colour.AdjustBrightness(tile.color, brightness);
		// tile.color = OffGridUtils.Colour.SetAlpha(tile.color, 255f);
		return tile;
	}
}