using Data;
using UnityEngine;

namespace Controllers
{
    public class MapController : MonoBehaviour
    {
        private PopulationMap _popMap;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _popMap = GetComponent<PopulationMap>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
        }

        public void Initialize() 
        {
            _popMap.Initialize();
        }
    }
}