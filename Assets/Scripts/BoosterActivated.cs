using UnityEngine;
using UnityEngine.UI;
public class BoosterActivated : MonoBehaviour
{
    public Player player;

    public Spawner spawner;

    public Slider boosterTimeSlider;

    private float timer;
    private float divider;

    private bool boosterActive = false;

    private void Update()
    {
        if (boosterActive)
        {
            timer -= Time.deltaTime / 3f;
            boosterTimeSlider.value = timer;

            if (timer <= 0)
            {
                BoosterDeactivateEffect();
            }
        }
    }


    public void BoosterActivateEffect()
    {
        timer = GameLogic.boosterTime;
        divider = GameLogic.boosterTime;
        boosterTimeSlider.maxValue = divider;
        timer = divider;
        boosterTimeSlider.value = divider;
        player.boosterActivatedMode = true;
        spawner.boosterActivatedMode = true;
        boosterActive = true;

        boosterTimeSlider.gameObject.SetActive(true);

        foreach (GameObject child in player.children)
        {
            child.GetComponent<BlockMovement>().ChangeToGold();
        }
        Time.timeScale = 3f;
    }

    public void BoosterDeactivateEffect()
    {
        GameObject.Find("SoundManager").GetComponent<SoundLogic>().BoosterStopSound();

        player.PlayBoosterDeactivateParticles();

        player.boosterActivatedMode = false;
        spawner.DeactivateBoosterEffect();
        boosterActive = false;

        boosterTimeSlider.value = divider;
        boosterTimeSlider.gameObject.SetActive(false);

        foreach (GameObject child in player.children)
        {
            Destroy(child);
        }
        player.children.Clear();

        timer = divider;

        Time.timeScale = 1f;
    }
}
