using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TapGameLogic : MonoBehaviour
{
    public SoundLogic soundLogic;

    public Camera cam;

    public Transform parent;

    public Button backButton;
    public Button[] upgradeButtons;

    public GameObject tapValuePrefab;
    public GameObject randomCeshPrefab;

    public GameObject boosterTimerSlider;

    public GameObject boosterActivateButton;
    public TextMeshProUGUI boosterAmountText;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI ceshText;

    public TextMeshProUGUI coinInfoText;
    public TextMeshProUGUI ceshInfoText;

    public TextMeshProUGUI tapValueCost;
    public TextMeshProUGUI ceshChanceCost;
    public Slider tapValueSlider;
    public Slider ceshChanceSlider;

    private float tapValue = 0.2f;
    private float ceshChance = 0.5f;
    private int tapGameBoosters = 0;

    private int tapValueStage = 0;
    private int ceshChanceStage = 0;

    private bool boosterActivated = false;

    private void Start()
    {
        LoadGame();
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //adding statistic
                Statistic.timesTapped++;
                //if cesh was spawned then end function to not spawn coin to (because of overlapping)
                if (RandomCesh())
                {
                    return;
                }

                AddCoins();

                SavingSystem.SavePlayer(GameLogic.instance);
            }      
        }

        //lessening booster's value to indicate time left of the boost
        if(boosterActivated)
        {
            boosterTimerSlider.GetComponent<Slider>().value -= Time.deltaTime / 5f;
        }

        //Mouse control instead:

        /*if (Input.GetMouseButtonDown(0))
        {
            //play sound
            soundLogic.TapValueSound();

            if (RandomCesh())
            {
                return;
            }

            AddCoins();
        }*/
    }

    public void MoreTapValue()
    {
        if (CheckMoreTapValueCost(tapValueStage))
        {
            tapValueStage++;
            UpdateTapValueStage();
        }

        coinText.text = System.Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();

        SaveGame();
    }
    private bool CheckMoreTapValueCost(int stage)
    {
        switch(stage)
        {
            case 0:
                if (GameLogic.coins >= 100)
                {
                    GameLogic.coins -= 100;
                    return true;
                }
                break;
            case 1:
                if (GameLogic.coins >= 1000)
                {
                    GameLogic.coins -= 1000;
                    return true;
                }
                break;
            case 2:
                if (GameLogic.coins >= 1500)
                {
                    GameLogic.coins -= 1500;
                    return true;
                }
                break;
            case 3:
                if (GameLogic.coins >= 3000)
                {
                    GameLogic.coins -= 3000;
                    return true;
                }
                break;
        }
        return false;
    }

    public void HigherCeshChance()
    {
        if(CheckHigherCeshChanceCost(ceshChanceStage))
        {
            ceshChanceStage++;
            UpdateCeshChanceStage();
        }

        coinText.text = System.Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();

        SaveGame();
    }

    private bool CheckHigherCeshChanceCost(int stage)
    {
        switch (stage)
        {
            case 0:
                if (GameLogic.cesh >= 10)
                {
                    GameLogic.cesh -= 10;
                    return true;
                }
                break;
            case 1:
                if (GameLogic.cesh >= 20)
                {
                    GameLogic.cesh -= 20;
                    return true;
                }
                break;
            case 2:
                if (GameLogic.cesh >= 30)
                {
                    GameLogic.cesh -= 30;
                    return true;
                }
                break;
            case 3:
                if (GameLogic.cesh >= 50)
                {
                    GameLogic.cesh -= 50;
                    return true;
                }
                break;
        }
        return false;
    }

    public void BoosterWatchAdd()
    {
        tapGameBoosters++;

        Statistic.adsWatched++;

        UpdateBoosterWatchAd();

        SaveGame();
    }

    public void UseBooster()
    {
        if (!boosterActivated) 
        {
            tapGameBoosters--;

            soundLogic.TapGameBoosterSound();

            UpdateBoosterWatchAd();

            StartCoroutine(WatchAdBoosterActivated());
        }

        SaveGame();
    }


    private bool RandomCesh()
    {
        int rand = Random.Range(1, 100);

        if (rand <= ceshChance) 
        {
            soundLogic.TapValueCeshSound();

            GameObject randomCeshInst = Instantiate(randomCeshPrefab, cam.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity, parent) as GameObject;
            randomCeshInst.transform.position = new Vector3(randomCeshInst.transform.position.x, randomCeshInst.transform.position.y, 0);

            GameLogic.cesh++;
            Statistic.ceshEarned++;
            ceshText.text = GameLogic.cesh.ToString();

            SaveGame();

            return true;
        }

        return false;
    }

    private void AddCoins()
    {
        soundLogic.TapValueSound();

        GameObject tapValueInst = Instantiate(tapValuePrefab, cam.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity, parent) as GameObject;
        tapValueInst.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + tapValue.ToString();
        tapValueInst.transform.position = new Vector3(tapValueInst.transform.position.x, tapValueInst.transform.position.y, 0);

        GameLogic.coins += tapValue;
        Statistic.coinsEarned += tapValue;
        coinText.text = System.Convert.ToInt32(GameLogic.coins).ToString();

        SaveGame();
    }

    private void LoadGame()
    {
        PlayerData data = SavingSystem.LoadPlayer();
        if (data != null)
        {
            tapValueStage = GameLogic.tapValueStage;
            ceshChanceStage = GameLogic.ceshChanceStage;
            tapGameBoosters = GameLogic.tapGameBoosters;
        }
        else return;

        UpdateTapValueStage();

        UpdateCeshChanceStage();

        UpdateBoosterWatchAd();
    }

    private void UpdateTapValueStage()
    {
        switch (tapValueStage)
        {
            case 0:
                tapValueSlider.value = 0;
                tapValueCost.text = "500";
                tapValue = 0.2f;
                break;
            case 1:
                tapValueSlider.value = 1;
                tapValueCost.text = "1000";
                tapValue = 0.4f;
                break;
            case 2:
                tapValueSlider.value = 2;
                tapValueCost.text = "1500";
                tapValue = 0.6f;
                break;
            case 3:
                tapValueSlider.value = 3;
                tapValueCost.text = "3000";
                tapValue = 0.8f;
                break;
            case 4:
                tapValueSlider.value = 4;
                tapValueCost.text = "MAX";
                tapValue = 1f;
                break;
        }
        coinInfoText.text = "+" + tapValue;
    }
    private void UpdateCeshChanceStage()
    {
        switch (ceshChanceStage)
        {
            case 0:
                ceshChanceSlider.value = 0;
                ceshChanceCost.text = "15";
                ceshChance = 1f;
                break;
            case 1:
                ceshChanceSlider.value = 1;
                ceshChanceCost.text = "20";
                ceshChance = 1.5f;
                break;
            case 2:
                ceshChanceSlider.value = 2;
                ceshChanceCost.text = "30";
                ceshChance = 2f;
                break;
            case 3:
                ceshChanceSlider.value = 3;
                ceshChanceCost.text = "50";
                ceshChance = 2.5f;
                break;
            case 4:
                ceshChanceSlider.value = 4;
                ceshChanceCost.text = "MAX";
                ceshChance = 3f;
                break;
        }
        ceshInfoText.text = "+" + ceshChance + "%";
    }
    private void UpdateBoosterWatchAd()
    {
        if (tapGameBoosters > 0)
        {
            boosterActivateButton.SetActive(true);
            boosterAmountText.text = "x" + tapGameBoosters;
        }
        else
        {
            boosterActivateButton.SetActive(false);
            boosterAmountText.text = "x" + tapGameBoosters;
        }
    }

    private void SaveGame()
    {
        GameLogic.tapGameBoosters = tapGameBoosters;
        GameLogic.tapValueStage = tapValueStage;
        GameLogic.ceshChanceStage = ceshChanceStage;

        SavingSystem.SavePlayer(GameLogic.instance);
    }

    IEnumerator WatchAdBoosterActivated()
    {
        backButton.interactable = false;
        for(int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].interactable = false;
        }
        float saveTapValue = tapValue;
        float saveCeshChance = ceshChance;
        tapValue = 5f;
        ceshChance = 5f;
        coinInfoText.text = "+" + tapValue;
        ceshInfoText.text = "+" + ceshChance + "%";
        boosterActivated = true;
        boosterTimerSlider.SetActive(true);
        yield return new WaitForSeconds(5f);
        tapValue = saveTapValue;
        ceshChance = saveCeshChance;
        coinInfoText.text = "+" + tapValue;
        ceshInfoText.text = "+" + ceshChance + "%";
        boosterActivated = false;
        boosterTimerSlider.GetComponent<Slider>().value = 1;
        boosterTimerSlider.SetActive(false);
        backButton.interactable = true;
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].interactable = true;
        }

        SaveGame();
    }
}
