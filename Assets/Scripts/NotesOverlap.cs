using UnityEngine;

public class NotesOverlap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Block"))
        {
            GameObject.Find("Player").GetComponent<Player>().children.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
