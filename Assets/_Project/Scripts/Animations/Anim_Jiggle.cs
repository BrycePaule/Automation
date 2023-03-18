using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public class Anim_Jiggle : MonoBehaviour
    {
        public Vector3 Jiggle;
        public float Speed;
        // public float AnimationTime;
        public bool SineSmoothing;

        private Vector3 startPos;
        private float progress;

        private void Awake()
        {
            startPos = transform.localPosition;
        }

        private void Update()
        {
            // float t = Time.time % AnimationTime;
            // float r = t < (AnimationTime / 2f) ? t : AnimationTime - t;
            // float pingpong = (Mathf.PingPong(Time.time, 1) * 2) - 1;
            // transform.localPosition = Vector3.Lerp(startPos, startPos + Jiggle, pingpong);

            if (SineSmoothing)
            {
                progress = Mathf.PingPong(Mathf.Sin(Time.time * Speed), 1f);
            }
            else
            {
                progress = Mathf.PingPong(Time.time * Speed, 1f);
            }

            transform.localPosition = Vector3.Lerp(startPos, startPos + Jiggle, progress);
        }
    }
}