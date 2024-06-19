using UnityEngine;

public class BlockSpeed : MonoBehaviour
{
    public float speed = 200f;

    private void FixedUpdate()
    {
        speed += Time.deltaTime;
        speed = Mathf.Clamp(speed, 200f, 325f);
    }

    public void ResetSpeed()
    {
        speed = 200f;
    }
}
