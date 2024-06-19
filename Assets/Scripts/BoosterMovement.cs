using UnityEngine;

public class BoosterMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    public Rigidbody2D rb;

    private void Awake()
    {
        //Destroy self after 5s to prevent lagging
        Invoke("SelfDestroy", 7f);
        //getting speed of BlockSpeed class
        speed = GameObject.Find("GameLogic").GetComponent<BlockSpeed>().speed;
    }

    private void FixedUpdate()
    {
        //movement
        rb.velocity = new Vector2(-speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.CompareTag("Player"))
            {
                Statistic.boostersPickedUp++;
                GameObject.Find("SoundManager").GetComponent<SoundLogic>().BoosterActivatedSound();
                GameObject.Find("GameLogic").GetComponent<BoosterActivated>().BoosterActivateEffect();
                SelfDestroy();
            }
            if(collision.CompareTag("Block"))
            {
                Destroy(gameObject);
            }
        }
    }

    //self destroy function
    private void SelfDestroy()
    {
        GameObject.Find("Player").GetComponent<Player>().boosters.RemoveAt(0);
        Destroy(gameObject);
    }
}
