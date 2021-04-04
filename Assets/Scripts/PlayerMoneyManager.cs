using UI;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    [SerializeField] private MoneyUI _moneyUi;
    private int _workers = 1;
    private int _money = 10000;
    private int _moneyPerWorker = 10;

    public int Money => _money;
    
    public static PlayerMoneyManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        InvokeRepeating("Generate", 1, 1);
    }

    private void Generate()
    {
        _money += _moneyPerWorker * _workers;
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
}
