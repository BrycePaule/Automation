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


        public static class SaveConversion
        {
            public static Dictionary<Vector3Int, int> ConvertEnumDictToInt<T>(Dictionary<Vector3Int, T> enumDict) where T : System.Enum
            {
                Dictionary<Vector3Int, int> intDict = new Dictionary<Vector3Int, int>();

                foreach (var item in enumDict)
                {
                    intDict[item.Key] = item.Value.GetHashCode();
                }

                return intDict;
            }

            // public static Dictionary<Vector3Int, T> ConvertIntEnumTo<T>(Dictionary<Vector3Int, int> intDict) where T : System.Enum
            // {
            //     Dictionary<Vector3Int, T> enumDict = new Dictionary<Vector3Int, T>();

            //     foreach (var item in intDict)
            //     {
            //         enumDict[item.Key] = (typeof(T)) item.Value;
            //     }

            //     return enumDict;
            // }


            // DICT <-> TUPLE LIST

            public static List<(TOne, TTwo)> DictToList<TOne, TTwo>(Dictionary<TOne, TTwo> dict)
            {
                List<(TOne, TTwo)> lst = new List<(TOne, TTwo)>();

                foreach (var item in dict)
                {
                    lst.Add((item.Key, item.Value));
                }

                return lst;
            }

            public static Dictionary<T1, T2> ListToDict<T1, T2>(List<(T1, T2)> lst)
            {
                Dictionary<T1, T2> dict = new Dictionary<T1, T2>();

                foreach (var item in lst)
                {
                    dict[item.Item1] = item.Item2;
                }

                return dict;
            }



        }

    }
}