using Assets.TerrainGenerator.Scripts;
using Assets.TerrainGenerator.Scripts.Algorithms;
using UnityEngine;

namespace Data
{
    public class PopulationMap : MonoBehaviour
    {
        private TerrainGenerator _generator;
        private HeightmapData _hmap;
        private float[,] _popMap;

        public void Initialize()
        {
            _generator = GetComponent<TerrainGenerator>();
            GenerateHeightmap();
            GeneratePopulationMap();
        }

        public void GenerateHeightmap()
        {
            _generator.GenerateTerrain();
        }

        public void GeneratePopulationMap() 
        {
        }

        public Texture2D GetAsTexture()
        {
            return _generator.HeightmapToImage();
        }
    }
}