using System.Linq;
using Game.Data;
using UnityEngine;
using Utility.BaseClasses;

namespace Game.Managers
{
    public class FranchiseManager : Singleton<FranchiseManager>
    {
        [SerializeField] private float _ticksPerSecond = 2f;
        [SerializeField] private float _profitPerPersonServedPerSecond = .33f / 24; // We're treating seconds as real-world days
        [SerializeField] private int _initialFunds = 10000;
        [SerializeField] private int _money = 0;
        private PoliticalMap _politicalMap;

        public int Money
        {
            get { return _money; }
            set { _money = value; }
        }

        public float ProfitPerPersonServedPerSecond
        {
            get { return _profitPerPersonServedPerSecond; }
        }

        public int TotalFranchises
        {
            get { return _politicalMap.CityControllers.Sum(city => city.City.FranchiseCount); }
        }

        public int ProfitPerSecond
        {
            get
            {
                return _politicalMap != null ? _politicalMap.CityControllers.Sum(city => city.GetProfitPerSecond()) : 0;
            }
        }
        
        protected FranchiseManager() { }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _politicalMap = FindObjectOfType<PoliticalMap>();

            Time.fixedDeltaTime = 1f / _ticksPerSecond;
            _money = _initialFunds;
        }
    }
}