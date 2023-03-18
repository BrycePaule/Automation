using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
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

        private void FixedUpdate()
        {
            if (TilemapManager.Instance.InsideBounds(InputManager.Instance.MPosCell))
            {
                Enable();
                UpdatePosition(InputManager.Instance.MPosCell);
            }
            else
            {
                Disable();
            }
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
}