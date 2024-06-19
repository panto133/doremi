using System;
using UnityEngine;
using TMPro;

public class ConvertingLogic : MonoBehaviour
{
    public static ConvertingLogic instance;

    private bool gameStarted = false;

    private int minExchangeValue = 50;
    private int maxExchangeValue = 60;

    private DateTime dateTimer;

    private float secondTimer = 59;

    public TextMeshProUGUI coinToCeshAmount;
    public TextMeshProUGUI ceshToCoinAmount;
    public TextMeshProUGUI possibleExchangeValueAmount;

    public TextMeshProUGUI timer;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI ceshText;

    private void Awake()
    {
        gameStarted = GameLogic.gameStarted;

        instance = this;
    }

    private void Start()
    {
        LoadGame();

        if(!gameStarted)
        {
            dateTimer = new DateTime
                (DateTime.Now.Year, 
                 DateTime.Now.Month, 
                 DateTime.Now.Day, 
                 DateTime.Now.Hour, 
                 5, 
                 59);
            RandomiseValues();
            SaveGame();
        }

        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
    }

    private void FixedUpdate()
    {
        Countdown();
        timer.text = dateTimer.Minute + ":" + dateTimer.Second;
    }

    private void RandomiseValues()
    {
        int rand1 = UnityEngine.Random.Range(minExchangeValue, maxExchangeValue);
        int rand2 = UnityEngine.Random.Range(100, 120);

        coinToCeshAmount.text = rand2.ToString();
        ceshToCoinAmount.text = rand1.ToString();
    }

    private void Countdown()
    {
        secondTimer -= Time.fixedDeltaTime;
        if(secondTimer <= 0)
        {
            if(dateTimer.Minute == 0)
            {
                dateTimer = new DateTime
                (DateTime.Now.Year,
                 DateTime.Now.Month,
                 DateTime.Now.Day,
                 DateTime.Now.Hour,
                 4,
                 59);

                secondTimer = 59;

                RandomiseValues();

                return;
            }

            dateTimer = new DateTime
                (DateTime.Now.Year,
                 DateTime.Now.Month,
                 DateTime.Now.Day,
                 DateTime.Now.Hour,
                 dateTimer.Minute - 1,
                 59);

            secondTimer = 59f;
            
        }

        dateTimer = dateTimer = new DateTime
                (DateTime.Now.Year,
                 DateTime.Now.Month,
                 DateTime.Now.Day,
                 DateTime.Now.Hour,
                 dateTimer.Minute,
                 Convert.ToInt16(secondTimer));

        SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void SaveGame()
    {
        GameLogic.dateTimer = dateTimer;

        GameLogic.coinToCeshAmount = Convert.ToInt32(coinToCeshAmount.text);
        GameLogic.ceshToCoinAmount = Convert.ToInt32(ceshToCoinAmount.text);


        SavingSystem.SavePlayer(GameLogic.instance);
    }
    private void LoadGame()
    {
        dateTimer = GameLogic.dateTimer;
        secondTimer = dateTimer.Second;

        coinToCeshAmount.text = GameLogic.coinToCeshAmount.ToString();
        ceshToCoinAmount.text = GameLogic.ceshToCoinAmount.ToString();
    }

    public void UpgradeMinExchangeValue(int amount)
    {
        minExchangeValue += amount;
        possibleExchangeValueAmount.text = minExchangeValue + "-" + maxExchangeValue;
    }

    public void UpgradeMaxExchangeValue(int amount)
    {
        maxExchangeValue += amount;
        possibleExchangeValueAmount.text = minExchangeValue + "-" + maxExchangeValue;
    }

    public void ExchangeCeshToCoin()
    {
        int textCoins = Convert.ToInt32(ceshToCoinAmount.text);
        if (GameLogic.cesh < 1) return;

        GameLogic.cesh--;
        GameLogic.coins += textCoins;

        Statistic.coinsEarned += Convert.ToInt32(ceshToCoinAmount.text);

        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
    }

    //Watch ad to get
    public void WatchAdForCoins()
    {
        GameLogic.coins += 100;

        Statistic.adsWatched++;
        Statistic.coinsEarned += 100;

        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
    }
    //Watch ad to get
    public void WatchAdForCesh()
    {
        GameLogic.cesh++;

        Statistic.adsWatched++;
        Statistic.ceshEarned++;

        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
    }

    public void ExchangeCoinsToCesh()
    {
        int textCoins = Convert.ToInt32(coinToCeshAmount.text);
        if (GameLogic.coins < textCoins) return;

        GameLogic.cesh++;
        GameLogic.coins -= textCoins;

        Statistic.ceshEarned++;

        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
    }

    //Buy with real money
    public void BuyCoinButton()
    {
        GameLogic.coins += 10000;

        Statistic.coinsEarned += 10000;

        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
    }
    //Buy with real money
    public void BuyCeshButton()
    {
        GameLogic.cesh += 100;

        Statistic.ceshEarned += 100;

        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
    }

}
