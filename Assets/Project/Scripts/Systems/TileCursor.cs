using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCursor : MonoBehaviour
{
    public bool Visible;
    public Sprite Sprite;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Sprite;
    }

    public void UpdatePosition(Vector3Int cellPos)
    {
        transform.position = TilemapManager.Instance.TileAnchorFromCellPos(cellPos);
    }

    public void Disable()
    {
        Visible = false;
        sr.enabled = false;
    }

    public void Enable()
    {
        Visible = true;
        sr.enabled = true;
    }
}
