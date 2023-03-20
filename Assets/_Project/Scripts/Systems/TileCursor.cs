using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public class TileCursor : Singleton<TileCursor>
    {
        public bool Visible;

        public CardinalDirection Direction;

        private SpriteRenderer sr;

        protected override void Awake()
        {
            base.Awake();

            sr = GetComponent<SpriteRenderer>();
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

        public void RotateClockwise(bool _reverse = false)
        {
            int _currDir = (int) Direction;
            int _newDir = (_currDir + 1) % 4;

            Vector3 _rotateVector = _reverse ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
            transform.Rotate(_rotateVector);

            Direction = (CardinalDirection) _newDir;
        }

        public void RotateAntiClockwise()
        {
            RotateClockwise(_reverse: true);
        }
    }
}