using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace bpdev
{
    public class MapGenerator : Singleton<MapGenerator>
    {
        [Header("Settings")]
        public scr_MapAsset MapAsset;
        public int MapSize { get; private set; }

        public PerlinSettings BaseSettings;
        public PerlinSettings Gem1Settings;
        public PerlinSettings Gem2Settings;

        [Header("Distance Scaling")]
        [SerializeField] private float distanceScaleValue; 

        private PerlinNoiseMapGenerator basePerlGen;
        private PerlinNoiseMapGenerator gem1PerlGen;
        private PerlinNoiseMapGenerator gem2PerlGen;
            
        private MapToken[,] blendedTokens;

        protected override void Awake()
        {
            base.Awake();
            MapSize = BaseSettings.MapSize;

            basePerlGen = new PerlinNoiseMapGenerator(BaseSettings);
            gem1PerlGen = new PerlinNoiseMapGenerator(Gem1Settings);
            gem2PerlGen = new PerlinNoiseMapGenerator(Gem2Settings);
        }

        private MapToken Blend(MapToken tBase, MapToken tGem1, MapToken tGem2)
        {
            if (tGem2 != MapToken.Empty) { return tGem2; }
            if (tGem1 != MapToken.Empty) { return tGem1; }

            return tBase;
        }

        public MapToken GetTokenAtPos(Vector3Int cellPos, Vector3Int playerOffset)
        {
            basePerlGen.Offset(playerOffset);
            gem1PerlGen.Offset(playerOffset);
            gem2PerlGen.Offset(playerOffset);

            MapToken tBase = basePerlGen.GetTokenAt(cellPos, BaseSettings.Tokens);
            MapToken tGem1 = gem1PerlGen.GetTokenAt(cellPos, Gem1Settings.Tokens, Mathf.Abs(cellPos.magnitude * distanceScaleValue));
            MapToken tGem2 = gem2PerlGen.GetTokenAt(cellPos, Gem2Settings.Tokens, Mathf.Abs(cellPos.magnitude * distanceScaleValue));

            return Blend(tBase, tGem1, tGem2);
        }

        // private scr_MapAsset GenerateNewMap()
        // {
        //     scr_MapAsset newMap = CreateMapAsset();

        //     blendedTokens = new MapToken[MapSize, MapSize];

        //     foreach (var point in Utils.EvaluateGrid(MapSize))
        //     {
        //         blendedTokens[point.y, point.x] = GetTokenAtPos(point, Vector3Int.zero);
        //     }

        //     string PATH = AssetDatabase.GenerateUniqueAssetPath("Assets/_Project/ScriptableObjects/Maps/map_NEWMAP.asset");
        //     AssetDatabase.CreateAsset(newMap, PATH);

        //     return newMap;
        // }

        public void RandomiseSeed()
        {
            float rand = Random.Range(0, 999);

            BaseSettings.Seed = rand;
            Gem1Settings.Seed = rand;
            Gem2Settings.Seed = rand;
        }

        // GIZMO

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.1f);
            float _offset = 0.5f;

            foreach (var point in Utils.EvaluateGrid(MapSize))
            {
                Vector3 _pos = new Vector3(point.x + _offset, point.y + _offset, 0);
                Gizmos.DrawWireCube( _pos, new Vector3(1, 1, 1));
            }
        }

        // MAP ASSET

        public scr_MapAsset CreateMapAsset()
        {
            scr_MapAsset newMap = scr_MapAsset.CreateInstance<scr_MapAsset>();

            newMap.PerlinSettings.Add(BaseSettings);
            newMap.PerlinSettings.Add(Gem1Settings);
            newMap.PerlinSettings.Add(Gem2Settings);

            string PATH = AssetDatabase.GenerateUniqueAssetPath("Assets/_Project/ScriptableObjects/Maps/map_NEWMAP.asset");
            AssetDatabase.CreateAsset(newMap, PATH);


            return newMap;
        }
    }

}