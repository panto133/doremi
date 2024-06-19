using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class GooglePlayGamesController : MonoBehaviour
{
    private void Start()
    {
        AuthenticateUser();
    }

    public void AuthenticateUser()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Logged into Google Play Games Services");
            }
            else
            {
                Debug.Log("Couldn't log into Google Play Games Services");
            }
        });
    }

    public static void PostToLeaderboard(int newScore)
    {
        Social.ReportScore(newScore, GPGSIds.leaderboard_high_score, (bool success) =>
        {
            if(success)
            {
                //Debug.Log("Successfully posted score to leaderboard");
            }
            else
            {
                //Debug.LogError("Failed to post score to leaderboard");
            }
        });
    }

    public static void ShowLeaderboardGUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score);
        }
        else
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Logged into Google Play Games Services");
                }
                else
                {
                    Debug.Log("Couldn't log into Google Play Games Services");
                }
            });
        }
    }
}
