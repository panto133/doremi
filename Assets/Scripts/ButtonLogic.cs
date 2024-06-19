using UnityEngine;
using System.Collections;
using TMPro;

public class ButtonLogic : MonoBehaviour
{
    public static ButtonLogic instance;

    public ShopInventoryLogic shopInventoryLogic;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI ceshText;

    public TextMeshProUGUI coinTextTapGame;
    public TextMeshProUGUI ceshTextTapGame;

    public TextMeshProUGUI coinTextConvertingMenu;
    public TextMeshProUGUI ceshTextConvertingMenu;

    public Player player;

    public GameLogic gameLogic;

    public GameObject menuCanvas;
    public GameObject shopCanvas;
    public GameObject inventoryCanvas;
    public GameObject tapGameCanvas;
    public GameObject gameScene;
    public GameObject convertingMenu;
    public GameObject statisticsMenu;
    public GameObject parallaxObjects;
    public GameObject tapGameParallaxObjects;
    public GameObject blackMask;

    public Animation mainCanvasAnimation;
    public Animation worldCanvasAnimation;

    public AnimationClip fromMenuToShop1;
    public AnimationClip fromMenuToShop2;

    public AnimationClip fromShopToMenu1;
    public AnimationClip fromShopToMenu2;

    public AnimationClip fromMenuToInventory1;
    public AnimationClip fromMenuToInventory2;

    public AnimationClip fromInventoryToMenu1;
    public AnimationClip fromInventoryToMenu2;

    public AnimationClip fromMainMenuToConvertingMenu1;
    public AnimationClip fromMainMenuToConvertingMenu2;

    public AnimationClip fromConvertingMenuToMainMenu1;
    public AnimationClip fromConvertingMenuToMainMenu2;

    public AnimationClip fromMainMenuToTapGame1;
    public AnimationClip fromMainMenuToTapGame2;

    public AnimationClip fromTapGameToMainMenu1;
    public AnimationClip fromTapGameToMainMenu2;

    public AnimationClip fromMainMenuToStatistics2;
    public AnimationClip fromStatisticsToMainMenu;

    public AnimationClip fromMainMenuToLeaderboard2;
    public AnimationClip fromLeaderboardToMainMenu1;

    public AnimationClip fromMainMenuToTapGamePO1;
    public AnimationClip fromMainMenuToTapGamePO2;

    public AnimationClip fromTapGameToMainMenuPO1;
    public AnimationClip fromTapGameToMainMenuPO2;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateValues();
    }

    public void PlayButton()
    {
        menuCanvas.SetActive(false);
        gameScene.SetActive(true);
        player.GameStarted();
        gameLogic.GameStarted();
    }

    public void FromMenuToShop()
    {
        StartCoroutine(FromMenuToShopAnimation());

        shopInventoryLogic.UpdateCCValues();
    }
    IEnumerator FromMenuToShopAnimation()
    {
        mainCanvasAnimation.clip = fromMenuToShop1;
        mainCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(false);
        shopCanvas.SetActive(true);
        mainCanvasAnimation.clip = fromMenuToShop2;
        mainCanvasAnimation.Play();
    }
    

    public void FromShopToMenu()
    {
        StartCoroutine(FromShopToMenuAnimation());

        UpdateValues();
    }
    IEnumerator FromShopToMenuAnimation()
    {
        for (int i = 0; i < shopInventoryLogic.infoHolders.Length; i++)
        {
            shopInventoryLogic.infoHolders[i].SetActive(false);
        }
        mainCanvasAnimation.clip = fromShopToMenu1;
        mainCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        mainCanvasAnimation.clip = fromShopToMenu2;
        mainCanvasAnimation.Play();
    }

    public void FromMenuToInventory()
    {
        StartCoroutine(FromMenuToInventoryAnimation());
    }
    IEnumerator FromMenuToInventoryAnimation()
    {
        mainCanvasAnimation.clip = fromMenuToInventory1;
        mainCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(false);
        inventoryCanvas.SetActive(true);
        mainCanvasAnimation.clip = fromMenuToInventory2;
        mainCanvasAnimation.Play();
    }

    public void FromMenuToStatistics()
    {
        StartCoroutine(FromMenuToStatisticsAnimation());
    }
    IEnumerator FromMenuToStatisticsAnimation()
    {
        mainCanvasAnimation.clip = fromMenuToInventory1;
        mainCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(false);
        statisticsMenu.SetActive(true);
        mainCanvasAnimation.clip = fromMainMenuToStatistics2;
        mainCanvasAnimation.Play();
    }

    public void FromStatisticsToMenu()
    {
        StartCoroutine(FromStatisticsToMenuAnimation());
    }
    IEnumerator FromStatisticsToMenuAnimation()
    {
        mainCanvasAnimation.clip = fromStatisticsToMainMenu;
        mainCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(true);
        statisticsMenu.SetActive(false);
        mainCanvasAnimation.clip = fromInventoryToMenu2;
        mainCanvasAnimation.Play();
    }

    public void FromMenuToLeaderboard()
    {
        GooglePlayGamesController.ShowLeaderboardGUI();
    }

    public void FromInventoryToMenu()
    {
        StartCoroutine(FromInventoryToMenuAnimation());

        UpdateValues();
    }
    IEnumerator FromInventoryToMenuAnimation()
    {
        mainCanvasAnimation.clip = fromInventoryToMenu1;
        mainCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(true);
        inventoryCanvas.SetActive(false);
        mainCanvasAnimation.clip = fromInventoryToMenu2;
        mainCanvasAnimation.Play();
    }

    public void FromMenuToTapGame()
    {
        StartCoroutine(FromMenuToTapGameAnimation());
    }
    IEnumerator FromMenuToTapGameAnimation()
    {
        mainCanvasAnimation.clip = fromMainMenuToTapGame1;
        mainCanvasAnimation.Play();
        worldCanvasAnimation.clip = fromMainMenuToTapGamePO1;
        worldCanvasAnimation.Play();
        worldCanvasAnimation.clip = fromMainMenuToTapGamePO2;
        worldCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(false);
        tapGameParallaxObjects.SetActive(true);
        tapGameCanvas.SetActive(true);
        blackMask.SetActive(false);
        parallaxObjects.SetActive(false);
        
        mainCanvasAnimation.clip = fromMainMenuToTapGame2;
        mainCanvasAnimation.Play();
    }

    public void FromTapGameToMenu()
    {
        StartCoroutine(FromTapGameToMenuAnimation());

        UpdateValues();
    }
    IEnumerator FromTapGameToMenuAnimation()
    {
        mainCanvasAnimation.clip = fromTapGameToMainMenu1;
        mainCanvasAnimation.Play();
        worldCanvasAnimation.clip = fromTapGameToMainMenuPO1;
        worldCanvasAnimation.Play();
        worldCanvasAnimation.clip = fromTapGameToMainMenuPO2;
        worldCanvasAnimation.Play();
        blackMask.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(true);
        tapGameParallaxObjects.SetActive(false);
        tapGameCanvas.SetActive(false);     
        parallaxObjects.SetActive(true); 
        mainCanvasAnimation.clip = fromTapGameToMainMenu2;
        mainCanvasAnimation.Play();
    }

    public void FromMainMenuToConvertingMenu()
    {
        StartCoroutine(FromMainMenuToConvertingMenuAnimation());

        UpdateValues();
    }
    IEnumerator FromMainMenuToConvertingMenuAnimation()
    {
        mainCanvasAnimation.clip = fromMainMenuToConvertingMenu1;
        mainCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(false);
        convertingMenu.SetActive(true);
        mainCanvasAnimation.clip = fromMainMenuToConvertingMenu2;
        mainCanvasAnimation.Play();
    }

    public void FromConvertingMenuToMainMenu()
    {
        StartCoroutine(FromConvertingMenuToMainMenuAnimation());

        UpdateValues();
    }
    IEnumerator FromConvertingMenuToMainMenuAnimation()
    {
        mainCanvasAnimation.clip = fromConvertingMenuToMainMenu1;
        mainCanvasAnimation.Play();

        yield return new WaitForSeconds(0.15f);

        menuCanvas.SetActive(true);
        convertingMenu.SetActive(false);
        mainCanvasAnimation.clip = fromConvertingMenuToMainMenu2;
        mainCanvasAnimation.Play();
    }

    public void UpdateValues()
    {
        coinText.text = System.Convert.ToInt32(GameLogic.coins).ToString();
        ceshText.text = GameLogic.cesh.ToString();

        coinTextTapGame.text = System.Convert.ToInt32(GameLogic.coins).ToString();
        ceshTextTapGame.text = GameLogic.cesh.ToString();

        coinTextConvertingMenu.text = System.Convert.ToInt32(GameLogic.coins).ToString();
        ceshTextConvertingMenu.text = GameLogic.cesh.ToString();
    }
}
