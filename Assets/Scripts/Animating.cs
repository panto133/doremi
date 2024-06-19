using System.Collections;
using UnityEngine;

public class Animating : MonoBehaviour
{
    public Animation handAppearingAnimation;
    public Animation satstmAnimation;
    public Animation handAnimation;
    public Animation pointDownAnimation;
    public Animation pointUpAnimation;

    public AnimationClip pointDownDisappearing;
    public AnimationClip pointUpDisappearing;
    public AnimationClip handDisappearing;
    public AnimationClip satstmDisappearing;

    public Spawner spawner;

    private void Start()
    {
        //start courutine of playing animations after everything is loaded
        StartCoroutine(PlayAnimations());
    }
    IEnumerator PlayAnimations()
    {
        //first segment (hand appearing, text appearing, arrow down appearing)
        handAppearingAnimation.Play();
        satstmAnimation.Play();
        handAnimation.Play();
        pointDownAnimation.Play();

        yield return new WaitForSeconds(1.5f);

        //second segment (point down disappearing, point up appearing)
        pointDownAnimation.clip = pointDownDisappearing;
        pointDownAnimation.Play();
        pointUpAnimation.Play();

        yield return new WaitForSeconds(1.5f);

        //third segment (point up disappearing)
        pointUpAnimation.clip = pointUpDisappearing;
        pointUpAnimation.Play();

        yield return new WaitForSeconds(1.5f);

        //fourth segment (everything disappearing (hand, text))
        handAppearingAnimation.clip = handDisappearing;
        handAppearingAnimation.Play();
        satstmAnimation.clip = satstmDisappearing;
        satstmAnimation.Play();
        spawner.enabled = true;

        //Destroy object to prevent bug collisions
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
