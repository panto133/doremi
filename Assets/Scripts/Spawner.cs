using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool boosterActivatedMode;

    public bool burstSpawned = false;

    public float distance = 0.6f;

    private int prevNote = -1;
    private int prevBooster = -1;

    private float timer = 0f;
    private float saveTimer = 0f;
    private float delay;
    private float boosterDelay;
    private float boosterTimer = 0f;
    private float boosterSaveTimer = 0f;
    private float boosterChance;
    private float randomTimer;

    private int randomChance;

    [SerializeField] private float defaultRandomTimer = 0.6f;
    [SerializeField] private float defaultDelay = 2f;
    [SerializeField] private float defaultBoosterDelay = 3f;
    [SerializeField] private float defaultBoosterChance = 5f;
    [SerializeField] private int defaultRandomChance = 20;

    public Player player;

    public GameObject blockPrefab;
    public GameObject boosterPrefab;

    public Sprite correctNoteImage;

    public Transform[] spawnPoints;

    public Transform parent;

    private void Awake()
    {
        //putting delay to what is set in inspector
        delay = defaultDelay;
        boosterDelay = defaultBoosterDelay;
        boosterChance = defaultBoosterChance;
        randomTimer = defaultRandomTimer;
        //setting random chance so that it doesn't burst at the beginning
        Invoke("SetRandomChance", 10f);
    }
    //Resetting speed when spawner starts spawning
    private void OnEnable()
    {
        GetComponent<BlockSpeed>().ResetSpeed();
    }
    private void FixedUpdate()
    {
        //lessening time between spawning of notes and booster if booster mode isn't activated
        if (!boosterActivatedMode)
        {
            delay -= Time.deltaTime / 100f;
            boosterDelay -= Time.deltaTime / 200f;

            saveTimer = timer;
            boosterSaveTimer = boosterTimer;
            boosterTimer += Time.deltaTime;
        }

        timer += Time.deltaTime;
        //limiting spawn time between distance and 2s
        delay = Mathf.Clamp(delay, distance, defaultDelay);
        boosterDelay = Mathf.Clamp(boosterDelay, distance, defaultBoosterDelay);

        //spawn notes and reset timer
        if (timer >= delay)
        {
            int rand = Random.Range(0,100);
            if (rand <= randomChance && !burstSpawned)
            {
                RandomBurstSpawn();
                burstSpawned = true;
            }
            else if(!burstSpawned)
            {
                SpawnBlock();
            }
            timer = 0f;
        }
        //spawning booster
        if(boosterTimer >= boosterDelay)
        {
            int rand = Random.Range(0, 100);
            if (rand <= boosterChance && !boosterActivatedMode && !burstSpawned)
            {
                SpawnBooster();
            }
            boosterTimer = 0f;
        }
    }

    public void DeactivateBoosterEffect()
    {
        boosterActivatedMode = false;
        timer = saveTimer;
        boosterTimer = boosterSaveTimer;
    }

    private void SetRandomChance()
    {
        randomChance = defaultRandomChance;
    }

    private void SpawnBlock()
    {
        //getting reference to instantiated block
        GameObject inst = Instantiate(blockPrefab, spawnPoints[1].position, Quaternion.identity, parent) as GameObject;
        //adding block to array list to be deleted later
        player.children.Add(inst);
        //generating random number to be correct note and prevents putting correct note in place of previous one
        int rand;
        do
        {
            rand = Random.Range(1, 7);
        }
        while (rand == prevNote);
        prevNote = rand;
        GameObject child = inst.transform.GetChild(rand - 1).gameObject;
        child.GetComponent<BoxCollider2D>().enabled = true;
        //changing color of correct note to be seen
        child.GetComponent<SpriteRenderer>().sprite = correctNoteImage;
        //adding correct tag to be properly validated when in collision with player
        child.tag = "Correct";

        //changing all children's color to gold if booster mode is activated
        if(boosterActivatedMode)
        {
            for(int i = 0; i < 6; i++)
            {
                inst.transform.GetComponentsInChildren<SpriteRenderer>()[i].sprite = correctNoteImage;
            }
        }    
    }

    private void SpawnBooster()
    {
        //prevents spawning booster on same spot as previous
        int rand;
        do
        {
            rand = Random.Range(0, 2);
        } while (rand == prevBooster);
        prevBooster = rand;
        GameObject inst = Instantiate(boosterPrefab, spawnPoints[rand].position, Quaternion.identity, parent) as GameObject;
        player.boosters.Add(inst);
    }

    public void ResetSpeed()
    {
        //resetting timers, chances and delays to their default values
        delay = defaultDelay;
        timer = 0;
        saveTimer = 0f;
        randomTimer = defaultRandomTimer;
        boosterTimer = 0;
        randomChance = 0;
        boosterChance = defaultBoosterChance;
        boosterDelay = defaultBoosterDelay;

        Invoke("SetRandomChance", 10f);
    }

    private void RandomBurstSpawn()
    {
        int rand = Random.Range(2, 5);

        switch(rand)
        {
            case 2:
                BurstSpawn2();
                break;
            case 3:
                BurstSpawn3();
                break;
            case 4:
                BurstSpawn4();
                break;
            default:
                BurstSpawn5();
                break;
        }

    }

    private void BurstSpawn2()
    {
        int rand = Random.Range(1, 3);
        int pos1, pos2, pos3;
        if(rand == 1)
        {
            int rand2 = Random.Range(2, 3);
            if (rand2 == 2) 
            {
                pos1 = 1;
                pos2 = 2;
                pos3 = 3;
            }
            else
            {
                pos1 = 1;
                pos2 = 3;
                pos3 = 2;
            }
        }
        else if (rand == 2)
        {
            int rand2 = Random.Range(2, 3);
            if (rand2 == 2)
            {
                pos1 = 2;
                pos2 = 1;
                pos3 = 3;
            }
            else
            {
                pos1 = 2;
                pos2 = 3;
                pos3 = 1;
            }
        }
        else
        {
            int rand2 = Random.Range(2, 3);
            if (rand2 == 2)
            {
                pos1 = 3;
                pos2 = 2;
                pos3 = 1;
            }
            else
            {
                pos1 = 3;
                pos2 = 1;
                pos3 = 2;
            }
        }
        StartCoroutine(BurstSpawn(pos1, pos2, pos3));
    }

    private void BurstSpawn3()
    {
        int rand = Random.Range(2, 4);
        int pos1, pos2, pos3;
        if (rand == 2)
        {
            int rand2 = Random.Range(3, 4);
            if (rand2 == 3)
            {
                pos1 = 2;
                pos2 = 3;
                pos3 = 4;
            }
            else
            {
                pos1 = 2;
                pos2 = 4;
                pos3 = 3;
            }
        }
        else if (rand == 3)
        {
            int rand2 = Random.Range(3, 4);
            if (rand2 == 3)
            {
                pos1 = 3;
                pos2 = 2;
                pos3 = 4;
            }
            else
            {
                pos1 = 3;
                pos2 = 4;
                pos3 = 2;
            }
        }
        else
        {
            int rand2 = Random.Range(3, 4);
            if (rand2 == 3)
            {
                pos1 = 4;
                pos2 = 3;
                pos3 = 2;
            }
            else
            {
                pos1 = 4;
                pos2 = 2;
                pos3 = 3;
            }
        }
        StartCoroutine(BurstSpawn(pos1, pos2, pos3));
    }

    private void BurstSpawn4()
    {
        int rand = Random.Range(3, 5);
        int pos1, pos2, pos3;
        if (rand == 3)
        {
            int rand2 = Random.Range(4, 5);
            if (rand2 == 4)
            {
                pos1 = 3;
                pos2 = 4;
                pos3 = 5;
            }
            else
            {
                pos1 = 3;
                pos2 = 5;
                pos3 = 4;
            }
        }
        else if (rand == 4)
        {
            int rand2 = Random.Range(4, 5);
            if (rand2 == 4)
            {
                pos1 = 4;
                pos2 = 3;
                pos3 = 5;
            }
            else
            {
                pos1 = 4;
                pos2 = 5;
                pos3 = 3;
            }
        }
        else
        {
            int rand2 = Random.Range(4, 5);
            if (rand2 == 4)
            {
                pos1 = 5;
                pos2 = 4;
                pos3 = 3;
            }
            else
            {
                pos1 = 5;
                pos2 = 3;
                pos3 = 4;
            }
        }
        StartCoroutine(BurstSpawn(pos1, pos2, pos3));
    }

    private void BurstSpawn5()
    {
        int rand = Random.Range(4, 6);
        int pos1, pos2, pos3;
        if (rand == 4)
        {
            int rand2 = Random.Range(5, 6);
            if (rand2 == 5)
            {
                pos1 = 4;
                pos2 = 5;
                pos3 = 6;
            }
            else
            {
                pos1 = 4;
                pos2 = 6;
                pos3 = 5;
            }
        }
        else if (rand == 5)
        {
            int rand2 = Random.Range(5, 6);
            if (rand2 == 5)
            {
                pos1 = 5;
                pos2 = 4;
                pos3 = 6;
            }
            else
            {
                pos1 = 5;
                pos2 = 6;
                pos3 = 4;
            }
        }
        else
        {
            int rand2 = Random.Range(5, 6);
            if (rand2 == 5)
            {
                pos1 = 6;
                pos2 = 5;
                pos3 = 4;
            }
            else
            {
                pos1 = 6;
                pos2 = 4;
                pos3 = 5;
            }
        }
        StartCoroutine(BurstSpawn(pos1, pos2, pos3));
    }

    IEnumerator BurstSpawn(int pos1, int pos2, int pos3)
    {
        GameObject inst = Instantiate(blockPrefab, spawnPoints[1].position, Quaternion.identity, parent) as GameObject;
        player.children.Add(inst);
        GameObject child = inst.transform.GetChild(pos1).gameObject;
        child.GetComponent<SpriteRenderer>().sprite = correctNoteImage;
        child.tag = "Correct";
        child.GetComponent<BoxCollider2D>().enabled = true;

        //If game has stopped then stop burst spawning
        if(!player.moveEnabled)
        {
            Destroy(inst);
            StopCoroutine(BurstSpawn(0,0,0));
        }
        //changing all children's color to gold if booster mode is activated
        if (boosterActivatedMode)
        {
            for (int i = 0; i < 6; i++)
            {
                inst.transform.GetComponentsInChildren<SpriteRenderer>()[i].sprite = correctNoteImage;
            }
        }
        yield return new WaitForSeconds(randomTimer);

        inst = Instantiate(blockPrefab, spawnPoints[1].position, Quaternion.identity, parent) as GameObject;
        player.children.Add(inst);
        child = inst.transform.GetChild(pos2).gameObject;
        child.GetComponent<SpriteRenderer>().sprite = correctNoteImage;
        child.tag = "Correct";
        child.GetComponent<BoxCollider2D>().enabled = true;

        //If game has stopped then stop burst spawning
        if (!player.moveEnabled)
        {
            Destroy(inst);
            StopCoroutine(BurstSpawn(0, 0, 0));
        }

        //changing all children's color to gold if booster mode is activated
        if (boosterActivatedMode)
        {
            for (int i = 0; i < 6; i++)
            {
                inst.transform.GetComponentsInChildren<SpriteRenderer>()[i].sprite = correctNoteImage;
            }
        }
        yield return new WaitForSeconds(randomTimer);

        inst = Instantiate(blockPrefab, spawnPoints[1].position, Quaternion.identity, parent) as GameObject;
        player.children.Add(inst);
        child = inst.transform.GetChild(pos3).gameObject;
        child.GetComponent<SpriteRenderer>().sprite = correctNoteImage;
        child.tag = "Correct";
        child.GetComponent<BoxCollider2D>().enabled = true;

        //After spawning burst setting burstSpawned to false so that booster can spawn
        burstSpawned = false;

        //changing all children's color to gold if booster mode is activated
        if (boosterActivatedMode)
        {
            for (int i = 0; i < 6; i++)
            {
                inst.transform.GetComponentsInChildren<SpriteRenderer>()[i].sprite = correctNoteImage;
            }
        }

        //If game has stopped then stop burst spawning
        if (!player.moveEnabled)
        {
            Destroy(inst);
            StopCoroutine(BurstSpawn(0, 0, 0));
        }

        timer -= randomTimer * 2f;
        timer = Mathf.Clamp(timer, 0, defaultDelay);
    }

    public void UpgradeBoosterChance()
    {
        defaultBoosterChance += 10;
    }
}
