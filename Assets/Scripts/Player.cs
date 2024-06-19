using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed = 250f;
    public float coinValue = 0.8f;
    private float modifier = 5f;
    private float direction;
    private float prevVelVal;
    private float summedVelocity;

    public bool boosterActivatedMode = false;
    public bool collided = false;

    public bool moveEnabled = false;
    private bool watchedAdd = false;
    private bool incomeSpawned = false;

    private int score = 0;
    private int failsBeforeInterstitialAd = 6;

    public List<GameObject> children = new List<GameObject>();
    public List<GameObject> boosters = new List<GameObject>();

    public AfterDeath afterDeath;
    public AdsManager adsManager;

    public Spawner spawner;
    public SoundLogic soundLogic;
    public ShopInventoryLogic shopInvLogic;

    public ParticleSystem playerDestroyedParticle;

    public GameObject coinIncomePrefab;
    public GameObject parent;
    public GameObject parentTapValues;
    public GameObject notesPlayedPrefab;
    public GameObject gameScene;
    public GameObject mainMenu;
    public GameObject blackMask;

    public Vector3 startPos;

    public TextMeshProUGUI scoreText;

    public Rigidbody2D rb;

    private Touch touch;   

    private void Awake()
    {
        children.Clear();
    }

    private void FixedUpdate()
    {
        //moveEnabled used for blocking movement while particles are showing
        if (moveEnabled)
        {
            Move();
        }
        //resetting speed after game over
        else
        {
            rb.velocity = Vector2.zero;
        }
        //Restart if reset all is queued and particle is done playing
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if (boosterActivatedMode)
            {
                AddScore();
                return;
            }

            if(collision.CompareTag("Correct"))
            {
                //playing the correct sound
                soundLogic.HitNote(collision.name);
                //Playing fade away animation
                collision.gameObject.transform.parent.GetComponent<BlockMovement>().PlayAnimation(collision.name);
                //clearing notes from list
                children.Remove(collision.gameObject);
                //letting second collider know it hit right note first
                collided = true;
                //adding score and then writing it on the tmpro
                AddScore();
            }
            if(collision.CompareTag("Block") && !collided)
            {
                //play sound
                soundLogic.PlayerDestroyedSound();
                Statistic.timesFailed++;
                failsBeforeInterstitialAd--;

                //show option to continue by watching the add if not watched
                if (!watchedAdd)
                {
                    afterDeath.ShowOptions();
                    StartCoroutine(GraphicGameOver());
                }
                //resetting the game
                else
                {
                    ResetAll();
                    StartCoroutine(ResetAdd());
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Block") || collision.CompareTag("Correct"))
            collided = false;
    }

    private void Move()
    {
        //direction scaled to fit the screen
        direction = Input.mousePosition.y / 108f - modifier;
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            modifier = Input.GetTouch(0).position.y / 108f;
        }
        //calculating the velocity
        summedVelocity = Mathf.Clamp(speed * direction * Time.deltaTime, -15f, 15f);
        if(transform.position.y > 4.05f && summedVelocity - prevVelVal > 0)
        {
            summedVelocity = 1f;
            direction = 0;
        }
        if(transform.position.y < -4.05f && summedVelocity - prevVelVal < 0)
        {
            direction = 0;
            summedVelocity = -1f;
        }
        rb.velocity = new Vector2(0, summedVelocity);
        prevVelVal = rb.velocity.y;
        //limiting the position to not go off screen
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.15f, 4.15f), 0);
    }

    //called by afterdeath to respawn after dying
    public void ContinueAfterAdd()
    {
        //disabling respawning again if ad was watched once
        watchedAdd = true;

        Statistic.timesContinued++;
        Statistic.adsWatched++;

        StopAnimations();
        Respawn();
    }

    private void StopAnimations()
    {
        playerDestroyedParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        soundLogic.StopPlayerDestroyedSound();

        foreach (GameObject child in children)
        {
            Destroy(child);
        }
        foreach (GameObject booster in boosters)
        {
            Destroy(booster);
        }

        children.Clear();
        boosters.Clear();
        spawner.enabled = true;
        transform.position = startPos;
        moveEnabled = true;
        Time.timeScale = 1f;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);

    }  

    //respawning player and enabling movement and spawner, not resetting speed called only from restart
    private void Respawn()
    {
        spawner.enabled = true;     
        moveEnabled = true;
        
        transform.position = startPos;
        GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    //not watching add, resetting speed called only from afterdeath
    public void Restart()
    {
        spawner.enabled = false;
        mainMenu.SetActive(true);
        blackMask.SetActive(true);
        soundLogic.PlayMenuMusic();

        foreach (GameObject child in children)
        {
            Destroy(child);
        }
        foreach (GameObject booster in boosters)
        {
            Destroy(booster);
        }

        children.Clear();
        boosters.Clear();
        ButtonLogic.instance.UpdateValues();
        shopInvLogic.CheckScoreSpecialSkins(score);
        GooglePlayGamesController.PostToLeaderboard(score);
        score = 0;
        incomeSpawned = false;
        scoreText.text = "Score: 0";
        GameObject.Find("GameLogic").GetComponent<BlockSpeed>().ResetSpeed();
        spawner.ResetSpeed();
        SavingSystem.SavePlayer(GameLogic.instance);

        gameScene.SetActive(false);
        if (failsBeforeInterstitialAd <= 0 && !shopInvLogic.noAdsBought)
        {
            adsManager.ShowAd();
            failsBeforeInterstitialAd = 6;
        }
    }

    //dying after watched add called only from on trigger enter if ad is already watched
    private void ResetAll()
    {
        StartCoroutine(GraphicGameOver());   
        StartCoroutine(WaitForParticlesToRestart());
    }

    //call restart function after particles are done playing
    IEnumerator WaitForParticlesToRestart()
    {
        yield return new WaitForSeconds(1f);
        Restart();
    }

    private void AddScore()
    {
        //adding score and then writing it on tmpro
        score++;
        Statistic.notesPlayed++;
        Statistic.coinsEarned += coinValue;
        GameLogic.coins += coinValue;
        scoreText.text = "Score: " + score;
        if (!incomeSpawned)
        {
            GameObject inst = Instantiate(coinIncomePrefab, new Vector3(transform.position.x, transform.position.y + 0.8f, 0), Quaternion.identity, parentTapValues.transform) as GameObject;
            inst.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = coinValue.ToString("0.0");
            incomeSpawned = true;
        }
        else
        {
            GameObject inst = Instantiate(notesPlayedPrefab, new Vector3(transform.position.x, transform.position.y + 0.8f, 0), Quaternion.identity, parentTapValues.transform) as GameObject;
        }
    }

    //called only from button logic when play button is pressed
    public void GameStarted()
    {
        moveEnabled = true;
        spawner.enabled = true;
        blackMask.SetActive(false);
        soundLogic.PlayLevelMusic();
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        Respawn();
    }

    public void PlayBoosterDeactivateParticles()
    {
        foreach(GameObject child in children)
        {
            child.transform.Find("Particle System").GetComponent<ParticleSystem>().Play();
            child.transform.Find("Particle System").parent = null;
        }
    }

    //resetting add var after waiting
    IEnumerator ResetAdd()
    {
        yield return new WaitForSeconds(3f);
        watchedAdd = false;
    }

    //show particles and destroy player and disable spawner (only visuals) called only from reset all
    IEnumerator GraphicGameOver()
    {
        playerDestroyedParticle.Play();

        GetComponent<BoxCollider2D>().enabled = false;
        moveEnabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        Time.timeScale = 0.333f;
        Time.fixedDeltaTime *= Time.timeScale;
        yield return new WaitForSeconds(1f);      
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        foreach (GameObject child in children)
        {
            Destroy(child);
        }
        foreach (GameObject booster in boosters)
        {
            Destroy(booster);
        }

        children.Clear();
        boosters.Clear();
    } 
}
