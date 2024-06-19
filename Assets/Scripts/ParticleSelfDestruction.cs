using UnityEngine;

public class ParticleSelfDestruction : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        Invoke("SelfDestroy", 7f);
        particle = GetComponent<ParticleSystem>();    
    }
    
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
