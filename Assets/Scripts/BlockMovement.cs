using UnityEngine;
using System.Collections.Generic;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    public Rigidbody2D rb;

    public Sprite correctNoteImage;

    public Animation blockAnimation;

    public AnimationClip RightNoteRe;
    public AnimationClip RightNoteMi;
    public AnimationClip RightNoteFa;
    public AnimationClip RightNoteSol;
    public AnimationClip RightNoteLa;
    public AnimationClip RightNoteSi;

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

    //self destroy function
    private void SelfDestroy()
    {
        List<GameObject> children = GameObject.Find("Player").GetComponent<Player>().children;
        children.Remove(gameObject);
        Destroy(gameObject);
    }

    //called from animation event to disable colliders after hitting right note
    public void DisableColliders()
    {
        BoxCollider2D[] list = transform.GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D child in list)
        {
            child.enabled = false;
        }
    }

    public void PlayAnimation(string name)
    {
        switch(name)
        {
            case "Re":
                blockAnimation.clip = RightNoteRe;
                break;

            case "Mi":
                blockAnimation.clip = RightNoteMi;
                break;

            case "Fa":
                blockAnimation.clip = RightNoteFa;
                break;

            case "Sol":
                blockAnimation.clip = RightNoteSol;
                break;

            case "La":
                blockAnimation.clip = RightNoteLa;
                break;

            default:
                blockAnimation.clip = RightNoteSi;
                break;
        }
        blockAnimation.Play();
    }

    public void ChangeToGold()
    {
        for (int i = 0; i < 6; i++)
        {
            transform.GetComponentsInChildren<SpriteRenderer>()[i].sprite = correctNoteImage;
        }
    }
}
