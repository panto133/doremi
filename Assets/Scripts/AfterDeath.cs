using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AfterDeath : MonoBehaviour
{
    private bool startCounting = false;
    private float timer = 3f;

    public Player player;

    public TextMeshProUGUI countdownTimer;

    public GameObject afterDeathGO;

    public Image loadingBar;

    private void FixedUpdate()
    {
        if(startCounting)
        {
            //loadingBar getting smaller
            loadingBar.fillAmount -= Time.deltaTime;
            //countdown timer inside loading bar
            timer -= Time.deltaTime * 3f;
            //displaying countdown timer
            countdownTimer.text = Mathf.Round(timer).ToString();

            if (loadingBar.fillAmount == 0)
            {
                //stop counting and reset vars
                StopCounting();
                //restart if player didn't choose to watch add
                player.Restart();
                //disable itself
                afterDeathGO.SetActive(false);
            }
        }  
    }

    //function called from player to show options and start counting
    public void ShowOptions()
    {
        afterDeathGO.SetActive(true);
        startCounting = true;
    }

    //function called from clicking a button to continue
    public void ContinueButton()
    {
        //respawning player after watching the add
        player.ContinueAfterAdd();
        StopCounting();
        afterDeathGO.SetActive(false);
    }

    //function to stop counting and reset vars
    private void StopCounting()
    {
        startCounting = false;
        loadingBar.fillAmount = 1f;
        timer = 3;
    }
}
