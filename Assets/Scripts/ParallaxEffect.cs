using UnityEngine;
using UnityEngine.UI;

public class ParallaxEffect : MonoBehaviour
{
    public float speed = 0;
    float pos = 0;
    private RawImage image;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        pos += speed;

        image.uvRect = new Rect(pos, 0, 1, 1);
    }
}
