using UI;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    [SerializeField] private MoneyUI _moneyUi;
    [SerializeField] private Transform _workersContainer;
    [SerializeField] private GameObject _moneyPopupPrefab;
    private int _workers = 1;
    private int _money = 1000;
    private int _moneyPerWorker = 5;

    public int Money => _money;
    
    public static PlayerMoneyManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        _money = DifficultyManager.Instance.Difficulty.StartingMoney;
        _moneyUi.UpdateText(_money);
        InvokeRepeating("Generate", 1, 1);
    }

    private void Generate()
    {
        if (!GameManager.Instance.IsActive) return;
        int money = _moneyPerWorker * _workers;
        MoneyPopup moneyPopup = SimplePool.Spawn(_moneyPopupPrefab, Vector3.zero, Quaternion.identity).GetComponent<MoneyPopup>();
        moneyPopup.Show(_workersContainer.position, money);
        _money += money;
        _moneyUi.UpdateText(_money);
    }

    public bool Remove(int money)
    {
        if (money > _money)
        {
            return false;
        }
        _money -= money;
        _moneyUi.UpdateText(_money);
        return true;
    }

    public void AddWorker()
    {
        _workers++;
        if (_workers > _workersContainer.childCount - 1) return;
        _workersContainer.GetChild(_workers).gameObject.SetActive(true);
    }
}
