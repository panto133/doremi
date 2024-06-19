using UnityEngine;

public class SelfDestructionTapValue : MonoBehaviour
{
    private void Awake()
    {
        Invoke("SelfDestroy", 2f);
    }
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
