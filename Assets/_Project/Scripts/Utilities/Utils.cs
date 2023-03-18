using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public static class Utils 
    {
        public static CardinalDirection IntToCardinalDirection(int number)
        {
            return (CardinalDirection) (number % 4);
        }

        public static Vector3Int DirToVector(CardinalDirection dir)
        {
            switch (dir)
            {
                case CardinalDirection.North:
                    return new Vector3Int(0, 1, 0);

                case CardinalDirection.East:
                    return new Vector3Int(1, 0, 0);

                case CardinalDirection.South:
                    return new Vector3Int(0, -1, 0);

                case CardinalDirection.West:
                    return new Vector3Int(-1, 0, 0);

                default:
                    return new Vector3Int(0, 0, 0);
            }
        }

        public static IEnumerable<Vector3Int> EvaluateGrid(int size)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    yield return new Vector3Int(x, y, 0);
                }
            }
        }

        public static IEnumerable<Vector3Int> EvaluateGrid(int xStart, int yStart, int size)
        {
            for (int y = yStart; y < yStart + size; y++)
            {
                for (int x = xStart; x < xStart + size; x++)
                {
                    yield return new Vector3Int(x, y, 0);
                }
            }
        }

        public static bool Roll(float chance)
        {
            return Random.Range(0f, 100f) <= chance;
        }

        public static class Colour
        {

            public static Color SetAlpha(Color colour, float alpha)
            {
                float r = colour.r;
                float g = colour.g;
                float b = colour.b;
                return new Color(r, g, b, alpha);
            }

            public static Color RandomColour()
            {
                return new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            }

            public static Color Darken(Color colour, float percent)
            {
                float h, s, v;
                Color.RGBToHSV(colour, out h, out s, out v);

                v -= v * percent;
                s -= s * percent;

                return Color.HSVToRGB(h, s, v);
            }
        }

    }
}