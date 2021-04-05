using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    #region Singleton

    public static DifficultyManager Instance { get; private set; }

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

        _easyDifficulty = new Difficulty();
        _easyDifficulty.StartingMoney = 2000;
        _easyDifficulty.TimeToSpawnTank = 50f;
        _easyDifficulty.TimeToSpawnSoldier = 30f;
        _easyDifficulty.QuantitySoldiers = 3;
        _easyDifficulty.QuantityTanks = 1;
        _easyDifficulty.TimeToMultiplyQuantity = 180;

        _normalDifficulty = new Difficulty();
        _normalDifficulty.StartingMoney = 1500;
        _normalDifficulty.TimeToSpawnTank = 40f;
        _normalDifficulty.TimeToSpawnSoldier = 25f;
        _normalDifficulty.QuantitySoldiers = 3;
        _normalDifficulty.QuantityTanks = 1;
        _normalDifficulty.TimeToMultiplyQuantity = 90;

        _hardDifficulty = new Difficulty();
        _hardDifficulty.StartingMoney = 1000;
        _hardDifficulty.TimeToSpawnTank = 30f;
        _hardDifficulty.TimeToSpawnSoldier = 20f;
        _hardDifficulty.QuantitySoldiers = 4;
        _hardDifficulty.QuantityTanks = 1;
        _hardDifficulty.TimeToMultiplyQuantity = 60;

        Difficulty = _easyDifficulty;
    }

    #endregion

    public Difficulty Difficulty { get; private set; }

    private Difficulty _easyDifficulty;
    private Difficulty _normalDifficulty;
    private Difficulty _hardDifficulty;

    public void SelectEasy()
    {
        Difficulty = _easyDifficulty;
    }

    public void SelectNormal()
    {
        Difficulty = _normalDifficulty;
    }

    public void SelectHard()
    {
        Difficulty = _hardDifficulty;
    }
}