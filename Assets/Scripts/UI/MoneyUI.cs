using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;

        public void UpdateText(int money)
        {
            _moneyText.text = $"$ {money}";
        }
    }
}