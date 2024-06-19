using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ShopInventoryLogic : MonoBehaviour
{
    public static ShopInventoryLogic instance;

    //Skins
    private bool gameStarted = false;

    private DateTime nowTime;

    public SpriteRenderer playerSprite;

    private bool[] skinsBought = new bool[10];
    private bool[] specialSkinsOwned = new bool[9];

    public Sprite[] skins;
    public Sprite[] specialSkins;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI ceshText;

    public Image[] inventorySkins;
    public Image[] specialInventorySkins;
    public GameObject[] equippedCheckmarks;
    public GameObject[] specialEquippedCheckmarks;
    public GameObject[] soldOutSigns;
    public GameObject[] specialSigns;
    public GameObject[] costHolders;

    public int[] costArray;

    public int currentlyEquippedIndex = 0;
    public int currentlySpecialEquippedIndex = -1;

    //Upgrades
    public Spawner spawner;
    public Player player;
    public ConvertingLogic convertingLogic;

    public Slider higherBoosterChanceSlider;
    public Slider higherMinExchangeValueSlider;
    public Slider higherMaxExchangeValueSlider;
    public Slider coinIncomeFromScoreSlider;
    public Slider increaseDistanceSlider;
    public Slider increaseBoosterTimeSlider;
    public Slider noAdsSlider;


    public int boosterChanceStage;
    public int minExchangeValueStage;
    public int maxExchangeValueStage;
    public int coinIncomeFromScoreStage;
    public int increaseDistanceStage;
    public int increaseBoosterTimeStage;
    public bool noAdsBought;

    public TextMeshProUGUI boosterChanceCost;
    public TextMeshProUGUI minExchangeCost;
    public TextMeshProUGUI maxExchangeCost;
    public TextMeshProUGUI coinIncomeFromScoreCost;
    public TextMeshProUGUI increaseDistanceCost;
    public TextMeshProUGUI increaseBoosterTimeCost;
    public TextMeshProUGUI noAdsCost;

    public Button noAdsButton;

    public GameObject[] infoHolders;
    private bool[] infoOpened = new bool[7];

    private void Start()
    {      
        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
        skinsBought[0] = true;

        if (!gameStarted)
        {
            nowTime = DateTime.Now;

            SaveGame();
        }

        CheckSpecialSkins();
    }

    private void Awake()
    {
        gameStarted = GameLogic.gameStarted;
        LoadGame();

        instance = this;
    }

    public void CheckSpecialSkins()
    {
        double diff = (DateTime.Now - nowTime).TotalDays;

        if (diff > 1 && !specialSkinsOwned[0])
        {
            UnlockSpecialSkin(0);
        }
        if (diff > 2 && !specialSkinsOwned[1])
        {
            UnlockSpecialSkin(1);
        }
        if (diff > 3 && !specialSkinsOwned[2])
        {
            UnlockSpecialSkin(2);
        }
        if (diff > 5 && !specialSkinsOwned[3])
        {
            UnlockSpecialSkin(3);
        }
        if (diff > 7 && !specialSkinsOwned[4])
        {
            UnlockSpecialSkin(4);
        }
        if (diff > 10 && !specialSkinsOwned[5])
        {
            UnlockSpecialSkin(5);
        }
    }
    public void CheckScoreSpecialSkins(int score)
    {
        if (score >= 100 && !specialSkinsOwned[6])
        {
            UnlockSpecialSkin(6);
        }
        if (score >= 200 && !specialSkinsOwned[7])
        {
            UnlockSpecialSkin(7);
        }
        if (score >= 300 && !specialSkinsOwned[8])
        {
            UnlockSpecialSkin(8);
        }
    }
    private void UnlockSpecialSkin(int index)
    {
        specialSkinsOwned[index] = true;
        specialInventorySkins[index].sprite = specialSkins[index];
        specialSigns[index].SetActive(false);

        Statistic.skinsOwned++;

        SaveGame();
    }

    public void EquipSkin(int index)
    {
        if (skinsBought[index])
        {
            if(currentlyEquippedIndex > -1)
                equippedCheckmarks[currentlyEquippedIndex].SetActive(false);

            if (currentlySpecialEquippedIndex > -1)
                specialEquippedCheckmarks[currentlySpecialEquippedIndex].SetActive(false);

            currentlySpecialEquippedIndex = -1;
            currentlyEquippedIndex = index;
            equippedCheckmarks[index].SetActive(true);
            playerSprite.sprite = skins[index];
        }

        SaveGame();
    }

    public void EquipSpecialSkin(int index)
    {
        if (GameLogic.specialSkinsOwned[index])
        {
            if(currentlySpecialEquippedIndex > -1)
                specialEquippedCheckmarks[currentlySpecialEquippedIndex].SetActive(false);

            if (currentlyEquippedIndex > -1)
                equippedCheckmarks[currentlyEquippedIndex].SetActive(false);

            currentlyEquippedIndex = -1;
            currentlySpecialEquippedIndex = index;
            specialEquippedCheckmarks[index].SetActive(true);
            playerSprite.sprite = specialSkins[index];
        }

        
        SaveGame();
    }

    public void BuySkin(int index)
    {
        if (skinsBought[index]) return;

        int cost = costArray[index];
        if (cost <= 100 && cost > GameLogic.cesh) return;
        if (cost > 100 && cost > GameLogic.coins) return;

        skinsBought[index] = true;
        soldOutSigns[index].SetActive(true);
        inventorySkins[index].sprite = skins[index];
        costHolders[index].SetActive(false);

        Statistic.skinsOwned++;

        if (cost <= 100) GameLogic.cesh -= cost;
        else GameLogic.coins -= cost;

        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();

        SaveGame();
    }
    public void LoadBoughtSkins(int index)
    {
        if (!skinsBought[index]) return;

        skinsBought[index] = true;
        soldOutSigns[index].SetActive(true);
        inventorySkins[index].sprite = skins[index];
        costHolders[index].SetActive(false);
    }
    public void LoadSpecialSkins(int index)
    {
        if (!specialSkinsOwned[index]) return;

        specialSkinsOwned[index] = true;
        specialSigns[index].SetActive(false);
        specialInventorySkins[index].sprite = specialSkins[index];
    }

    private void SaveGame()
    {
        GameLogic.currentlyEquippedIndex = currentlyEquippedIndex;
        GameLogic.currentlySpecialEquippedIndex = currentlySpecialEquippedIndex;

        GameLogic.skinsBought = skinsBought;
        GameLogic.specialSkinsOwned = specialSkinsOwned;

        GameLogic.boosterChanceStage = boosterChanceStage;
        GameLogic.minExchangeValueStage = minExchangeValueStage;
        GameLogic.maxExchangeValueStage = maxExchangeValueStage;
        GameLogic.coinIncomeFromScoreStage = coinIncomeFromScoreStage;
        GameLogic.increaseDistanceStage = increaseDistanceStage;
        GameLogic.increaseBoosterTimeStage = increaseBoosterTimeStage;
        GameLogic.noAdsBought = noAdsBought;

        GameLogic.nowTime = nowTime;

        SavingSystem.SavePlayer(GameLogic.instance);
    }

    private void LoadGame()
    {
        PlayerData data = SavingSystem.LoadPlayer();

        if(data != null)
        {
            boosterChanceStage = GameLogic.boosterChanceStage;
            minExchangeValueStage = GameLogic.minExchangeValueStage;
            maxExchangeValueStage = GameLogic.maxExchangeValueStage;
            coinIncomeFromScoreStage = GameLogic.coinIncomeFromScoreStage;
            increaseDistanceStage = GameLogic.increaseDistanceStage;
            increaseBoosterTimeStage = GameLogic.increaseBoosterTimeStage;
            noAdsBought = GameLogic.noAdsBought;

            nowTime = GameLogic.nowTime;

            skinsBought = GameLogic.skinsBought;
            specialSkinsOwned = GameLogic.specialSkinsOwned;

            equippedCheckmarks[currentlyEquippedIndex].SetActive(false);
            currentlyEquippedIndex = GameLogic.currentlyEquippedIndex;
            currentlySpecialEquippedIndex = GameLogic.currentlySpecialEquippedIndex;

            for (int i = 1; i < skinsBought.Length; i++)
            {
                LoadBoughtSkins(i);
            }
            for (int i = 0; i < specialSkinsOwned.Length; i++)
            {
                LoadSpecialSkins(i);
            }

            if (currentlyEquippedIndex > -1)
            {
                EquipSkin(currentlyEquippedIndex);
            }
            else
            {
                EquipSpecialSkin(currentlySpecialEquippedIndex);
            }

            CheckBoosterChanceValue(true);
            CheckHigherMinExchangeValue(true);
            CheckHigherMaxExchangeValue(true);
            CheckHigherCoinIncomeFromScore(true);
            CheckIncreaseDistance(true);
            CheckIncreaseBoosterTime(true);
            CheckNoAds(true);
        }
    }

    public void UpdateCCValues()
    {
        coinText.text = Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();
    }

    public void BuyHigherBoosterChance()
    {
        if(CheckBoosterChanceValue(false))
        {
            spawner.UpgradeBoosterChance();
        }

        SaveGame();
        UpdateCCValues();
    }
    private bool CheckBoosterChanceValue(bool fromFile)
    {
        if (!fromFile)
        {
            switch (boosterChanceStage)
            {
                case 0:
                    if (GameLogic.coins >= 1000)
                    {
                        boosterChanceStage = 1;
                        GameLogic.coins -= 1000;
                        boosterChanceCost.text = "2000";
                        higherBoosterChanceSlider.value = 1;
                        return true;
                    }
                    break;
                case 1:
                    if (GameLogic.coins >= 2000)
                    {
                        boosterChanceStage = 2;
                        GameLogic.coins -= 2000;
                        boosterChanceCost.text = "3000";
                        higherBoosterChanceSlider.value = 2;
                        return true;
                    }
                    break;
                case 2:
                    if (GameLogic.coins >= 3000)
                    {
                        boosterChanceStage = 3;
                        GameLogic.coins -= 3000;
                        boosterChanceCost.text = "4000";
                        higherBoosterChanceSlider.value = 3;
                        return true;
                    }
                    break;
                case 3:
                    if (GameLogic.coins >= 4000)
                    {
                        boosterChanceStage = 4;
                        GameLogic.coins -= 4000;
                        boosterChanceCost.text = "MAX";
                        higherBoosterChanceSlider.value = 4;
                        return true;
                    }
                    break;
            }
        }
        else
        {
            switch (boosterChanceStage)
            {
                case 0:
                    boosterChanceStage = 0;
                    boosterChanceCost.text = "1000";
                    higherBoosterChanceSlider.value = 0;
                    break;
                case 1:
                    boosterChanceStage = 1;
                    boosterChanceCost.text = "2000";
                    higherBoosterChanceSlider.value = 1;
                    break;
                case 2:
                    boosterChanceStage = 2;
                    boosterChanceCost.text = "3000";
                    higherBoosterChanceSlider.value = 2;
                    break;
                case 3:
                    boosterChanceStage = 3;
                    boosterChanceCost.text = "4000";
                    higherBoosterChanceSlider.value = 3;
                    break;
                case 4:
                    boosterChanceStage = 4;
                    boosterChanceCost.text = "MAX";
                    higherBoosterChanceSlider.value = 4;
                    break;
            }
        }
        return false;
    }

    public void BuyHigherMinExchangeValue()
    {
        if (CheckHigherMinExchangeValue(false))
        {
            convertingLogic.UpgradeMinExchangeValue(10);
            
        }

        SaveGame();
        UpdateCCValues();
    }
    private bool CheckHigherMinExchangeValue(bool fromFile)
    {
        if (!fromFile)
        {
            switch (minExchangeValueStage)
            {
                case 0:
                    if (GameLogic.coins >= 500)
                    {
                        minExchangeValueStage = 1;
                        GameLogic.coins -= 500;
                        minExchangeCost.text = "1000";
                        higherMinExchangeValueSlider.value = 1;
                        return true;
                    }
                    break;
                case 1:
                    if (GameLogic.coins >= 1500)
                    {
                        minExchangeValueStage = 2;
                        GameLogic.coins -= 1500;
                        minExchangeCost.text = "2000";
                        higherMinExchangeValueSlider.value = 2;
                        return true;
                    }
                    break;
                case 2:
                    if (GameLogic.coins >= 2000)
                    {
                        minExchangeValueStage = 3;
                        GameLogic.coins -= 2000;
                        minExchangeCost.text = "2500";
                        higherMinExchangeValueSlider.value = 3;
                        return true;
                    }
                    break;
                case 3:
                    if (GameLogic.coins >= 2500)
                    {
                        minExchangeValueStage = 4;
                        GameLogic.coins -= 2500;
                        minExchangeCost.text = "MAX";
                        higherMinExchangeValueSlider.value = 4;
                        return true;
                    }
                    break;
            }
        }
        else
        {
            switch (minExchangeValueStage)
            {
                case 0:
                    minExchangeValueStage = 0;
                    minExchangeCost.text = "500";
                    higherMinExchangeValueSlider.value = 0;
                    break;
                case 1:
                    minExchangeValueStage = 1;
                    minExchangeCost.text = "1500";
                    higherMinExchangeValueSlider.value = 1;
                    convertingLogic.UpgradeMinExchangeValue(10);
                    break;
                case 2:
                    minExchangeValueStage = 2;
                    minExchangeCost.text = "2000";
                    higherMinExchangeValueSlider.value = 2;
                    convertingLogic.UpgradeMinExchangeValue(20);
                    break;
                case 3:
                    minExchangeValueStage = 3;
                    minExchangeCost.text = "2500";
                    higherMinExchangeValueSlider.value = 3;
                    convertingLogic.UpgradeMinExchangeValue(30);
                    break;
                case 4:
                    minExchangeValueStage = 4;
                    minExchangeCost.text = "MAX";
                    higherMinExchangeValueSlider.value = 4;
                    convertingLogic.UpgradeMinExchangeValue(40);
                    break;
            }
        }
        return false;
    }

    public void BuyHigherMaxExchangeValue()
    {
        if(CheckHigherMaxExchangeValue(false))
        {
            convertingLogic.UpgradeMaxExchangeValue(10);
        }

        SaveGame();
        UpdateCCValues();
    }
    private bool CheckHigherMaxExchangeValue(bool fromFile)
    {
        if (!fromFile)
        {
            switch (maxExchangeValueStage)
            {
                case 0:
                    if (GameLogic.coins >= 1000)
                    {
                        maxExchangeValueStage = 1;
                        GameLogic.coins -= 1000;
                        maxExchangeCost.text = "1500";
                        higherMaxExchangeValueSlider.value = 1;                       
                        return true;
                    }
                    break;
                case 1:
                    if (GameLogic.coins >= 1500)
                    {
                        maxExchangeValueStage = 2;
                        GameLogic.coins -= 1500;
                        maxExchangeCost.text = "2000";
                        higherMaxExchangeValueSlider.value = 2;
                        return true;
                    }
                    break;
                case 2:
                    if (GameLogic.coins >= 2000)
                    {
                        maxExchangeValueStage = 3;
                        GameLogic.coins -= 2000;
                        maxExchangeCost.text = "2500";
                        higherMaxExchangeValueSlider.value = 3;
                        return true;
                    }
                    break;
                case 3:
                    if (GameLogic.coins >= 2500)
                    {
                        maxExchangeValueStage = 4;
                        GameLogic.coins -= 2500;
                        maxExchangeCost.text = "MAX";
                        higherMaxExchangeValueSlider.value = 4;
                        return true;
                    }
                    break;
            }
        }
        else
        {
            switch (maxExchangeValueStage)
            {
                case 0:
                    maxExchangeValueStage = 0;
                    maxExchangeCost.text = "1000";
                    higherMaxExchangeValueSlider.value = 0;
                    break;
                case 1:
                    maxExchangeValueStage = 1;
                    maxExchangeCost.text = "1500";
                    higherMaxExchangeValueSlider.value = 1;
                    convertingLogic.UpgradeMaxExchangeValue(10);
                    break;
                case 2:
                    maxExchangeValueStage = 2;
                    maxExchangeCost.text = "2000";
                    higherMaxExchangeValueSlider.value = 2;
                    convertingLogic.UpgradeMaxExchangeValue(20);
                    break;
                case 3:
                    maxExchangeValueStage = 3;
                    maxExchangeCost.text = "2500";
                    higherMaxExchangeValueSlider.value = 3;
                    convertingLogic.UpgradeMaxExchangeValue(30);
                    break;
                case 4:
                    maxExchangeValueStage = 4;
                    maxExchangeCost.text = "MAX";
                    higherMaxExchangeValueSlider.value = 4;
                    convertingLogic.UpgradeMaxExchangeValue(40);
                    break;
            }
        }
        return false;
    }

    public void BuyHigherCoinIncomeFromScore()
    {
        if(CheckHigherCoinIncomeFromScore(false))
        {
            player.coinValue += 0.1f;
        }

        SaveGame();
        UpdateCCValues();
    }
    private bool CheckHigherCoinIncomeFromScore(bool fromFile)
    {
        if (!fromFile)
        {
            switch (coinIncomeFromScoreStage)
            {
                case 0:
                    if (GameLogic.coins >= 300)
                    {
                        coinIncomeFromScoreStage = 1;
                        GameLogic.coins -= 300;
                        coinIncomeFromScoreCost.text = "600";
                        coinIncomeFromScoreSlider.value = 1;
                        return true;
                    }
                    break;
                case 1:
                    if (GameLogic.coins >= 600)
                    {
                        coinIncomeFromScoreStage = 2;
                        GameLogic.coins -= 600;
                        coinIncomeFromScoreCost.text = "900";
                        coinIncomeFromScoreSlider.value = 2;
                        return true;
                    }
                    break;
                case 2:
                    if (GameLogic.coins >= 900)
                    {
                        coinIncomeFromScoreStage = 3;
                        GameLogic.coins -= 900;
                        coinIncomeFromScoreCost.text = "1500";
                        coinIncomeFromScoreSlider.value = 3;
                        return true;
                    }
                    break;
                case 3:
                    if (GameLogic.coins >= 1500)
                    {
                        coinIncomeFromScoreStage = 4;
                        GameLogic.coins -= 1500;
                        coinIncomeFromScoreCost.text = "MAX";
                        coinIncomeFromScoreSlider.value = 4;
                        return true;
                    }
                    break;
            }
        }
        else
        {
            switch (coinIncomeFromScoreStage)
            {
                case 0:
                     coinIncomeFromScoreStage = 0;
                     coinIncomeFromScoreCost.text = "300";
                     coinIncomeFromScoreSlider.value = 0;
                     player.coinValue = 0.6f;
                    break;
                case 1:
                    coinIncomeFromScoreStage = 1;
                    coinIncomeFromScoreCost.text = "600";
                    coinIncomeFromScoreSlider.value = 1;
                    player.coinValue = 0.7f;
                    break;
                case 2:
                    coinIncomeFromScoreStage = 2;
                    coinIncomeFromScoreCost.text = "900";
                    coinIncomeFromScoreSlider.value = 2;
                    player.coinValue = 0.8f;
                    break;
                case 3:
                    coinIncomeFromScoreStage = 3;
                    coinIncomeFromScoreCost.text = "1500";
                    coinIncomeFromScoreSlider.value = 3;
                    player.coinValue = 0.9f;
                    break;
                case 4:
                    coinIncomeFromScoreStage = 4;
                    coinIncomeFromScoreCost.text = "MAX";
                    coinIncomeFromScoreSlider.value = 4;
                    player.coinValue = 1f;
                    break;
            }
        }
        return false;
    }

    public void BuyIncreaseDistance()
    {
        if(CheckIncreaseDistance(false))
        {
            spawner.distance += 0.05f;
        }

        SaveGame();
        UpdateCCValues();
    }
    private bool CheckIncreaseDistance(bool fromFile)
    {
        if(!fromFile)
        {
            switch(increaseDistanceStage)
            {
                case 0:
                    if (GameLogic.coins >= 3500)
                    {
                        increaseDistanceStage = 1;
                        GameLogic.coins -= 3500;
                        increaseDistanceCost.text = "7000";
                        increaseDistanceSlider.value = 1;
                        return true;
                    }
                    break;
                case 1:
                    if (GameLogic.coins >= 7000)
                    {
                        increaseDistanceStage = 2;
                        GameLogic.coins -= 7000;
                        increaseDistanceCost.text = "10500";
                        increaseDistanceSlider.value = 2;
                        return true;
                    }
                    break;
                case 2:
                    if (GameLogic.coins >= 10500)
                    {
                        increaseDistanceStage = 3;
                        GameLogic.coins -= 10500;
                        increaseDistanceCost.text = "14000";
                        increaseDistanceSlider.value = 3;
                        return true;
                    }
                    break;
                case 3:
                    if (GameLogic.coins >= 14000)
                    {
                        increaseDistanceStage = 4;
                        GameLogic.coins -= 14000;
                        increaseDistanceCost.text = "MAX";
                        increaseDistanceSlider.value = 4;
                        return true;
                    }
                    break;
            }
        }
        else
        {
            switch(increaseDistanceStage)
            {
                case 0:
                    increaseDistanceStage = 0;
                    increaseDistanceCost.text = "3500";
                    increaseDistanceSlider.value = 0;
                    spawner.distance = 0.6f;
                    break;
                case 1:
                    increaseDistanceStage = 1;
                    increaseDistanceCost.text = "7000";
                    increaseDistanceSlider.value = 1;
                    spawner.distance = 0.65f;
                    break;
                case 2:
                    increaseDistanceStage = 2;
                    increaseDistanceCost.text = "10500";
                    increaseDistanceSlider.value = 2;
                    spawner.distance = 0.7f;
                    break;
                case 3:
                    increaseDistanceStage = 3;
                    increaseDistanceCost.text = "14000";
                    increaseDistanceSlider.value = 3;
                    spawner.distance = 0.75f;
                    break;
                case 4:
                    increaseDistanceStage = 4;
                    increaseDistanceCost.text = "MAX";
                    increaseDistanceSlider.value = 4;
                    spawner.distance = 0.8f;
                    break;
            }
        }

        return false;
    }

    public void BuyIncreaseBoosterTime()
    {
        if (CheckIncreaseBoosterTime(false))
        {
            GameLogic.boosterTime += 0.5f;
        }

        SaveGame();
        UpdateCCValues();
    }
    private bool CheckIncreaseBoosterTime(bool fromFile)
    {
        if (!fromFile)
        {
            switch (increaseBoosterTimeStage)
            {
                case 0:
                    if (GameLogic.coins >= 2000)
                    {
                        increaseBoosterTimeStage = 1;
                        GameLogic.coins -= 2000;
                        increaseBoosterTimeCost.text = "4000";
                        increaseBoosterTimeSlider.value = 1;
                        return true;
                    }
                    break;
                case 1:
                    if (GameLogic.coins >= 4000)
                    {
                        increaseBoosterTimeStage = 2;
                        GameLogic.coins -= 4000;
                        increaseBoosterTimeCost.text = "6000";
                        increaseBoosterTimeSlider.value = 2;
                        return true;
                    }
                    break;
                case 2:
                    if (GameLogic.coins >= 6000)
                    {
                        increaseBoosterTimeStage = 3;
                        GameLogic.coins -= 6000;
                        increaseBoosterTimeCost.text = "8000";
                        increaseBoosterTimeSlider.value = 3;
                        return true;
                    }
                    break;
                case 3:
                    if (GameLogic.coins >= 8000)
                    {
                        increaseBoosterTimeStage = 4;
                        GameLogic.coins -= 8000;
                        increaseBoosterTimeCost.text = "MAX";
                        increaseBoosterTimeSlider.value = 4;
                        return true;
                    }
                    break;
            }
        }
        else
        {
            switch (increaseBoosterTimeStage)
            {
                case 0:
                    increaseBoosterTimeStage = 0;
                    increaseBoosterTimeCost.text = "2000";
                    increaseBoosterTimeSlider.value = 0;
                    GameLogic.boosterTime = 2f;
                    break;
                case 1:
                    increaseBoosterTimeStage = 1;
                    increaseBoosterTimeCost.text = "4000";
                    increaseBoosterTimeSlider.value = 1;
                    GameLogic.boosterTime = 2.5f;
                    break;
                case 2:
                    increaseBoosterTimeStage = 2;
                    increaseBoosterTimeCost.text = "6000";
                    increaseBoosterTimeSlider.value = 2;
                    GameLogic.boosterTime = 3f;
                    break;
                case 3:
                    increaseBoosterTimeStage = 3;
                    increaseBoosterTimeCost.text = "8000";
                    increaseBoosterTimeSlider.value = 3;
                    GameLogic.boosterTime = 3.5f;
                    break;
                case 4:
                    increaseBoosterTimeStage = 4;
                    increaseBoosterTimeCost.text = "MAX";
                    increaseBoosterTimeSlider.value = 4;
                    GameLogic.boosterTime = 4f;
                    break;
            }
        }

        return false;
    }

    public void BuyNoAds()
    {
        if(CheckNoAds(false))
        {
            noAdsBought = true;

            noAdsButton.interactable = false;
        }

        SaveGame();
        UpdateCCValues();
    }
    private bool CheckNoAds(bool fromFile)
    {
        if(!fromFile)
        {
            noAdsSlider.value = 4;
            noAdsCost.text = "MAX";
            noAdsBought = true;

            return true;
        }
        else
        {
            noAdsBought = GameLogic.noAdsBought;
            if (noAdsBought)
            {
                noAdsSlider.value = 4;
                noAdsCost.text = "MAX";
                noAdsButton.interactable = false;
            }
        }

        return false;
    }

    public void ShowInfo(int index)
    {
        for (int i = 0; i < infoOpened.Length; i++)
        {
            if (infoOpened[i] && i != index)
            {
                infoHolders[i].SetActive(false);
                infoOpened[i] = false;
            }
        }
        
        if(infoOpened[index])
        {
            infoHolders[index].SetActive(false);
            infoOpened[index] = false;
            return;
        }

        infoHolders[index].SetActive(true);
        infoOpened[index] = true;
    }
}
