using System;
using UnityEngine;
using TMPro;
public class StatisticsManager : MonoBehaviour
{
    public TextMeshProUGUI notesPlayedText;
    public TextMeshProUGUI timePlayedText;
    public TextMeshProUGUI coinsEarnedText;
    public TextMeshProUGUI ceshEarnedText;
    public TextMeshProUGUI adsWatchedText;
    public TextMeshProUGUI boostersPickedUpText;
    public TextMeshProUGUI timesFailedText;
    public TextMeshProUGUI timesContinuedText;
    public TextMeshProUGUI skinsOwnedText;
    public TextMeshProUGUI timesTappedText;

    private void OnEnable()
    {
        WriteStatistics();
    }

    private void WriteStatistics()
    {
        notesPlayedText.text = "Notes Played: " + Statistic.notesPlayed;
        timePlayedText.text = "Time Played: " + string.Format("{0:0.0}", Statistic.timePlayed) + "h";
        coinsEarnedText.text = "Coins Earned: " + Convert.ToInt32(Statistic.coinsEarned);
        ceshEarnedText.text = "Cesh Earned: " + Statistic.ceshEarned;
        adsWatchedText.text = "Ads Watched: " + Statistic.adsWatched;
        boostersPickedUpText.text = "Boosters Picked Up: " + Statistic.boostersPickedUp;
        timesFailedText.text = "Times Failed: " + Statistic.timesFailed;
        timesContinuedText.text = "Times Continued: " + Statistic.timesContinued;
        skinsOwnedText.text = "Skins Owned: " + Statistic.skinsOwned;
        timesTappedText.text = "Times Tapped: " + Statistic.timesTapped;
}
}
