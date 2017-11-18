using Game.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Controllers
{
    public class CityInfoController : MonoBehaviour
    {
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _populationText;
        [SerializeField] private Text _potentialCustomersText;
        [SerializeField] private Text _customersServedText;
        [SerializeField] private Text _franchiseText;
        [SerializeField] private Text _profitRateText;
        [SerializeField] private Text _nextProfitRateText;
        [SerializeField] private Text _nextCostText;
        private CityController _cityController;

        private void Awake()
        {
            _infoPanel.SetActive(false);
        }

        public void Initialize(CityController controller)
        {
            _cityController = controller;
        }

        public void Open()
        {
            SetPosition();
            _infoPanel.SetActive(true);
        }

        public void Close()
        {
            _infoPanel.SetActive(false);
        }

        public void NewFranchise()
        {
            _cityController.AddFranchise();
        }

        private void Update()
        {
            if (_infoPanel.activeInHierarchy)
            {
                _nameText.text = _cityController.City.Name;
                _populationText.text = string.Format("Pop: {0}", _cityController.City.Population);
                _potentialCustomersText.text = string.Format("Pot Cust: {0}", _cityController.GetPotentialCustomers());
                _customersServedText.text = string.Format("Cust Served: {0}%", Mathf.FloorToInt(_cityController.GetCustomersServedRatio() * 100));
                _franchiseText.text = string.Format("Franchises: {0}", _cityController.City.FranchiseCount);
                _profitRateText.text = string.Format("Profit/Sec: ${0}", _cityController.GetProfitPerSecond());
                _nextProfitRateText.text = string.Format("Next Franchise\nProfit/Sec: ${0}", _cityController.GetNextProfitPerSecond());
                _nextCostText.text = string.Format("${0}", _cityController.GetNextFranchiseCost());
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    _cityController.LeftClickOut();
                }
            }
        }

        private void SetPosition()
        {
            // Make sure the menu stays entirely within the screen
            RectTransform rect = GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            Vector2 screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
            Vector2 screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            if (corners[0].x < screenMin.x)
            {
                rect.position = new Vector3((screenMin.x * rect.lossyScale.x * rect.pivot.x), rect.position.y, rect.position.z);
            }
            if (corners[2].x > screenMax.x)
            {
                rect.position = new Vector3(screenMax.x - (rect.rect.width * rect.lossyScale.x * rect.pivot.x), rect.position.y, rect.position.z);
            }
            if (corners[1].y > screenMax.y)
            {
                rect.position = new Vector3(rect.position.x, screenMax.y - (rect.rect.height * rect.lossyScale.y * rect.pivot.y), rect.position.z);
            }
            if (corners[0].y < screenMin.y)
            {
                rect.position = new Vector3(rect.position.x, screenMin.y + (rect.rect.height * rect.lossyScale.y * rect.pivot.y), rect.position.z);
            }
        }
    }
}