using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class City
    {
        [SerializeField] public string Name;
        [SerializeField] public int Population;
        [SerializeField] public Vector2 Position;
        [SerializeField] public float CostFactor; // How expensive the city is
        [SerializeField] public float InterestFactor = .01f; // How likely each person is to eat there
        [SerializeField] public int FranchiseCount = 0;

        public City(string name, int population, float costFactor, Vector2 position)
        {
            Name = name;
            Population = population;
            CostFactor = costFactor;
            Position = position;
        }
    }
}