using Game.Data;
using Game.Managers;
using UI.Controllers;
using UnityEngine;

namespace Game.Controllers
{
    public class CityController : MonoBehaviour
    {
        [SerializeField] private GameObject _infoPanelPrefab;
        [SerializeField] public City City;
        [SerializeField] private int _initialFranchiseCost = 5000;
        [SerializeField] private int _maxServedPerFranchise = 50000;
        private Collider2D _collider;
        private TextMesh _nameText;
        private MeshRenderer _nameRenderer;
        private CityInfoController _infoUI;

        private bool _active = false;

        private void Awake()
        {
            Instantiate(_infoPanelPrefab, transform);
            _collider = GetComponent<Collider2D>();
            _infoUI = GetComponentInChildren<CityInfoController>();
        }

        public void Initialize(City city)
        {
            City = city;
            name = city.Name;
            _nameText = GetComponentInChildren<TextMesh>();
            _nameRenderer = _nameText.GetComponent<MeshRenderer>();
            _nameText.text = city.Name;
            _infoUI.Initialize(this);
        }

        private void FixedUpdate()
        {
            FranchiseManager.Instance.Money += Mathf.FloorToInt(GetProfitPerSecond() * Time.deltaTime);
        }

        public void AddFranchise()
        {
            int cost = GetNextFranchiseCost();
            if (FranchiseManager.Instance.Money >= cost)
            {
                FranchiseManager.Instance.Money -= cost;
                City.FranchiseCount++;
            }
        }

        public int GetNextFranchiseCost()
        {
            int cost = 0;
            if (City.FranchiseCount == 0)
            {
                cost += _initialFranchiseCost;
            }
            else
            {
                cost += 1000;
            }
//            cost += Mathf.FloorToInt(Mathf.Pow(City.FranchiseCount, .75f) * 1000);
            cost = Mathf.FloorToInt(cost * City.CostFactor);
            // Exponentially increase cost
            return cost;
        }

        public int GetPotentialCustomers()
        {
            return Mathf.FloorToInt(City.Population * City.InterestFactor);
        }

        public int GetCustomersServed()
        {
            if (City.FranchiseCount == 0) return 0;
            return Mathf.Min(
                Mathf.Min(
                    Mathf.CeilToInt((float)GetPotentialCustomers() / City.FranchiseCount),
                    _maxServedPerFranchise
                ) * City.FranchiseCount,
                GetPotentialCustomers()
            );
        }

        public float GetCustomersServedRatio()
        {
            if (GetPotentialCustomers() == 0) return 0;
            return Mathf.Min((float)GetCustomersServed() / (float)GetPotentialCustomers(), 100);
        }

        public int GetProfitPerSecond()
        {
            if (City.FranchiseCount == 0) return 0;
            return Mathf.FloorToInt(
                // People served
                GetCustomersServed()
                // How much people will pay for food in the city
                * City.CostFactor
                // How much people will pay for the franchise's food per second
                * FranchiseManager.Instance.ProfitPerPersonServedPerSecond
            );
        }

        public int GetNextProfitPerSecond()
        {
            return Mathf.FloorToInt(
                // People served
                Mathf.Min(
                    Mathf.Min(
                        Mathf.CeilToInt((float)GetPotentialCustomers() / (City.FranchiseCount + 1)),
                        _maxServedPerFranchise
                    ) * (City.FranchiseCount + 1),
                    GetPotentialCustomers()
                )
                // How much people will pay for food in the city
                * City.CostFactor
                // How much people will pay for the franchise's food per second
                * FranchiseManager.Instance.ProfitPerPersonServedPerSecond
            );
        }

        public void LeftClickOut()
        {
            _infoUI.Close();
            _active = false;
        }

        private void OnMouseEnter()
        {
            if (!_active)
            {
                _nameRenderer.enabled = true;
            }
        }

        private void OnMouseExit()
        {
            _nameRenderer.enabled = false;
        }

        private void OnMouseUpAsButton()
        {
            _infoUI.Open();
            _active = true;
        }
    }
}