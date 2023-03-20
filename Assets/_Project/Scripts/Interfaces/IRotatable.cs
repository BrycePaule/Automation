using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public interface ITSystemRotatable
    {
        public void RotateClockwise(bool _reverse);
        public void RotateAntiClockwise();

        public void RotateToFace(CardinalDirection targetDir);
    }
}
