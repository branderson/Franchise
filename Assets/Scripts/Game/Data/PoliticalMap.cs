using System.Collections.Generic;
using System.Linq;
using Game.Controllers;
using UnityEngine;

namespace Game.Data
{
    public class PoliticalMap : MonoBehaviour
    {
        [SerializeField] private GameObject _cityPrefab;
        private Dictionary<string, City> _cities;
        public List<CityController> CityControllers;

        private void Start()
        {
            List<City> cityList = new List<City>
            {
                new City("New York", 8175133, 5, new Vector2(26.53f, 6.55f)),
                new City("Los Angeles", 3792621, 3, new Vector2(-26.58f, -3.51f)),
                new City("Chicago", 2695598, 1.5f, new Vector2(9.95f, 5.37f)),
                new City("Houston", 2100263, 1f, new Vector2(2.38f, -13.94f)),
                new City("Phoenix", 1445632, 1f, new Vector2(-20.07f, -6.15f)),
                new City("San Francisco", 805235, 4, new Vector2(-30.77f, 2.97f)),
                new City("Seattle", 608660, 3, new Vector2(-26.13f, 17.46f)),
                new City("Boston", 617594, 2, new Vector2(29.42f, 8.84f)),
                new City("Miami", 399457, 2, new Vector2(22.67f, -18.25f)),
                new City("Denver", 600158, 1.5f, new Vector2(-10.02f, 2.3f)),
            };
            _cities = cityList.ToDictionary(city => city.Name, city => city);
            foreach (City city in _cities.Values)
            {
                GameObject instantiated = Instantiate(
                    _cityPrefab,
                    city.Position,
                    Quaternion.identity,
                    transform
                );
                CityController controller = instantiated.GetComponent<CityController>();
                controller.Initialize(city);
                CityControllers.Add(controller);
            }
        }
    }
}