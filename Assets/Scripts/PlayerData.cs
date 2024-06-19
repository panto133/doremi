using System;
[Serializable]
public class PlayerData
{
    public bool playTutorial;
    public bool gameStarted;
    public bool[] skinsBought = new bool[10];
    public bool[] specialSkinsOwned = new bool[9];
    public bool musicOn;
    public bool soundOn;

    public float coins;
    public int cesh;

    public int tapValueStage;
    public int ceshChanceStage;
    public int tapGameBoosters;
    public int currentlyEquippedIndex;
    public int currentlySpecialEquippedIndex;

    public int boosterChanceStage;
    public int minExchangeValueStage;
    public int maxExchangeValueStage;
    public int coinIncomeFromScoreStage;
    public int increaseDistanceStage;
    public int increaseBoosterTimeStage;
    public bool noAdsBought;
    public int coinToCeshAmount;
    public int ceshToCoinAmount;

    public int notesPlayed;
    public double timePlayed;
    public float coinsEarned;
    public int ceshEarned;
    public int adsWatched;
    public int boostersPickedUp;
    public int timesFailed;
    public int timesContinued;
    public int skinsOwned;
    public int timesTapped;

    public int adsWatchedForTheDay;
    public DateTime timeFirstAd;

    public DateTime dateTimer;
    public DateTime nowTime;

    public PlayerData(GameLogic gameLogic)
    {
        playTutorial = GameLogic.playTutorial;
        gameStarted = GameLogic.gameStarted;
        musicOn = GameLogic.musicOn;
        soundOn = GameLogic.soundOn;

        coins = GameLogic.coins;
        cesh = GameLogic.cesh;

        tapValueStage = GameLogic.tapValueStage;
        ceshChanceStage = GameLogic.ceshChanceStage;
        tapGameBoosters = GameLogic.tapGameBoosters;

        currentlyEquippedIndex = GameLogic.currentlyEquippedIndex;
        currentlySpecialEquippedIndex = GameLogic.currentlySpecialEquippedIndex;

        skinsBought = GameLogic.skinsBought;
        specialSkinsOwned = GameLogic.specialSkinsOwned;

        boosterChanceStage = GameLogic.boosterChanceStage;
        minExchangeValueStage = GameLogic.minExchangeValueStage;
        maxExchangeValueStage = GameLogic.maxExchangeValueStage;
        coinIncomeFromScoreStage = GameLogic.coinIncomeFromScoreStage;
        increaseDistanceStage = GameLogic.increaseDistanceStage;
        increaseBoosterTimeStage = GameLogic.increaseBoosterTimeStage;
        noAdsBought = GameLogic.noAdsBought;

        dateTimer = GameLogic.dateTimer;
        nowTime = GameLogic.nowTime;

        coinToCeshAmount = GameLogic.coinToCeshAmount;
        ceshToCoinAmount = GameLogic.ceshToCoinAmount;

        adsWatchedForTheDay = GameLogic.adsWatchedForTheDay;
        timeFirstAd = GameLogic.timeFirstAd;

        notesPlayed = Statistic.notesPlayed;
        timePlayed = Statistic.timePlayed;
        coinsEarned = Statistic.coinsEarned;
        ceshEarned = Statistic.ceshEarned;
        adsWatched = Statistic.adsWatched;
        boostersPickedUp = Statistic.boostersPickedUp;
        timesFailed = Statistic.timesFailed;
        timesContinued = Statistic.timesContinued;
        skinsOwned = Statistic.skinsOwned;
        timesTapped = Statistic.timesTapped;
}
}
