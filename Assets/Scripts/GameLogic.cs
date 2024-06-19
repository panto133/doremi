using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public static bool playTutorial = true;
    public static bool gameStarted = false;

    public GameObject canvasTutorial;

    public Sprite soundOnIcon;
    public Sprite soundOffIcon;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    public Image soundButton;
    public Image musicButton;

    public AudioSource soundAud;
    public AudioSource musicAud;

    public static GameLogic instance;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI ceshText;

    public static bool[] skinsBought = new bool[10];
    public static bool[] specialSkinsOwned = new bool[9];
    public static bool musicOn = true;
    public static bool soundOn = true;
    public static float coins = 0;
    public static int cesh = 0;
    public static int tapGameBoosters = 0;
    public static int tapValueStage = 0;
    public static int ceshChanceStage = 0;
    public static int currentlyEquippedIndex = 0;
    public static int currentlySpecialEquippedIndex = -1;
    public static int boosterChanceStage;
    public static int minExchangeValueStage;
    public static int maxExchangeValueStage;
    public static int coinIncomeFromScoreStage;
    public static int increaseDistanceStage;
    public static int increaseBoosterTimeStage;
    public static bool noAdsBought;
    public static int coinToCeshAmount;
    public static int ceshToCoinAmount;
    //max is 7 per 12h
    public static int adsWatchedForTheDay;
    public static DateTime timeFirstAd;

    public static DateTime dateTimer;
    public static DateTime nowTime;

    public static float boosterTime = 2f;

    //variables for calculating time spent in game
    private DateTime start;
    private DateTime end;
    private TimeSpan startEndSpan;

    //scaling var
    public RectTransform worldCanvasRt;

    private void Awake()
    {
        instance = this;
        LoadGame();

        float canvasHeight = worldCanvasRt.rect.height;
        float desiredCanvasWidth = canvasHeight * Camera.main.aspect;
        worldCanvasRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, desiredCanvasWidth);
    }
    private void Start()
    {
        start = DateTime.Now;
        end = DateTime.Now;

        gameStarted = true;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Home))
        {
            Application.Quit();
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            end = DateTime.Now;
            Statistic.timePlayed += (end - start).TotalHours;
            SavingSystem.SavePlayer(instance);
        }
        else
        {
            start = DateTime.Now;
            SavingSystem.SavePlayer(instance);
        }
    }

    private void OnApplicationQuit()
    {
        end = DateTime.Now;

        Statistic.timePlayed += (end - start).TotalHours;

        SavingSystem.SavePlayer(instance);
    }

    private void PlayTutorial()
    {
        canvasTutorial.SetActive(true);
    }

    public void GameStarted()
    {
        //if playTutorial is true play tutorial
        if (playTutorial)
        {
            PlayTutorial();
            playTutorial = false;
        }
        //else start spawning 
        else
        {
            GetComponent<Spawner>().enabled = true;
        }
        //save changes to playTutorial var if made any
        SavingSystem.SavePlayer(this);
    }

    public void SoundEnable()
    {
        soundOn = !soundOn;

        if(soundOn)
        {
            soundAud.mute = false;
            soundButton.sprite = soundOnIcon;
        }
        else
        {
            soundAud.mute = true;
            soundButton.sprite = soundOffIcon;
        }

        SavingSystem.SavePlayer(instance);
    }
    public void MusicEnable()
    {
        musicOn = !musicOn;

        if(musicOn)
        {
            musicAud.mute = false;
            musicButton.sprite = musicOnIcon;
        }
        else
        {
            musicAud.mute = true;
            musicButton.sprite = musicOffIcon;
        }

        SavingSystem.SavePlayer(instance);
    }

    private void LoadGame()
    {
        PlayerData data = SavingSystem.LoadPlayer();
        //if data exists check its vars
        if (data != null)
        {
            //getting var from data
            playTutorial = data.playTutorial;
            gameStarted = data.gameStarted;
            musicOn = data.musicOn;
            soundOn = data.soundOn;

            LoadSoundSettings();

            coins = data.coins;
            cesh = data.cesh;

            tapGameBoosters = data.tapGameBoosters;
            tapValueStage = data.tapValueStage;
            ceshChanceStage = data.ceshChanceStage;
            currentlyEquippedIndex = data.currentlyEquippedIndex;
            currentlySpecialEquippedIndex = data.currentlySpecialEquippedIndex;

            skinsBought = data.skinsBought;
            specialSkinsOwned = data.specialSkinsOwned;

            coinText.text = coins.ToString();
            ceshText.text = cesh.ToString();
            boosterChanceStage = data.boosterChanceStage;
            minExchangeValueStage = data.minExchangeValueStage;
            maxExchangeValueStage = data.maxExchangeValueStage;
            coinIncomeFromScoreStage = data.coinIncomeFromScoreStage;
            increaseDistanceStage = data.increaseDistanceStage;
            increaseBoosterTimeStage = data.increaseBoosterTimeStage;
            noAdsBought = data.noAdsBought;
            dateTimer = data.dateTimer;
            nowTime = data.nowTime;
            coinToCeshAmount = data.coinToCeshAmount;
            ceshToCoinAmount = data.ceshToCoinAmount;

            adsWatchedForTheDay = data.adsWatchedForTheDay;
            timeFirstAd = data.timeFirstAd;

            Statistic.notesPlayed = data.notesPlayed;
            Statistic.timePlayed = data.timePlayed;
            Statistic.coinsEarned = data.coinsEarned;
            Statistic.ceshEarned = data.ceshEarned;
            Statistic.adsWatched = data.adsWatched;
            Statistic.boostersPickedUp = data.boostersPickedUp;
            Statistic.timesFailed = data.timesFailed;
            Statistic.timesContinued = data.timesContinued;
            Statistic.skinsOwned = data.skinsOwned;
            Statistic.timesTapped = data.timesTapped;
        }
    }
    private void LoadSoundSettings()
    {
        if(soundOn)
        {
            soundButton.sprite = soundOnIcon;
            soundAud.mute = false;
        }
        else
        {
            soundButton.sprite = soundOffIcon;
            soundAud.mute = true;
        }

        if(musicOn)
        {
            musicButton.sprite = musicOnIcon;
            musicAud.mute = false;
        }
        else
        {
            musicButton.sprite = musicOffIcon;
            musicAud.mute = true;
        }
    }
}
