using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controllers
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] private Text _moneyText;
        [SerializeField] private Text _franchiseCountText;
        [SerializeField] private Text _profitRateText;

        private void Update()
        {
            _moneyText.text = string.Format("Funds: ${0}", FranchiseManager.Instance.Money);
            _franchiseCountText.text = string.Format("Franchises: {0}", FranchiseManager.Instance.TotalFranchises);
            _profitRateText.text = string.Format("Profit/Sec: ${0}", FranchiseManager.Instance.ProfitPerSecond);
        }
    }
}