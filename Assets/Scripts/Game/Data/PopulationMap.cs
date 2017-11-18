using TerrainGenerator.Scripts;
using UnityEngine;

namespace Game.Data
{
    public class PopulationMap : MonoBehaviour
    {
        private TerrainGenerator.Scripts.TerrainGenerator _generator;
        private HeightmapData _hmap;
        private float[,] _popMap;

        public void Initialize()
        {
            _generator = GetComponent<TerrainGenerator.Scripts.TerrainGenerator>();
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