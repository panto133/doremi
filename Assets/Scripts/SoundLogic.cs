using UnityEngine;

public class SoundLogic : MonoBehaviour
{
    public AudioSource soundAud;
    public AudioSource musicAud;

    public AudioClip[] menuMusic;
    public AudioClip levelMusic;

    public AudioClip buttonClick;
    public AudioClip tapCoin;
    public AudioClip tapCesh;
    public AudioClip[] playedNote;

    public AudioClip re;
    public AudioClip mi;
    public AudioClip fa;
    public AudioClip sol;
    public AudioClip la;
    public AudioClip si;
    public AudioClip playerDestroyed;

    public AudioClip hpre;
    public AudioClip hpmi;
    public AudioClip hpfa;
    public AudioClip hpsol;
    public AudioClip hpla;
    public AudioClip hpsi;

    public AudioClip bbre;
    public AudioClip bbmi;
    public AudioClip bbfa;
    public AudioClip bbsol;
    public AudioClip bbla;
    public AudioClip bbsi;

    public AudioClip boosterActivatedSound;
    public AudioClip tapGameBooster;

    private bool menuMusicPlaying = false;
    private bool levelMusicPlaying = false;

    private int prevMenuMusic = -1;

    public void ButtonClickSound()
    {
        soundAud.PlayOneShot(buttonClick);
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    private void Update()
    {
        if(menuMusicPlaying && !musicAud.isPlaying)
        {
            PlayMenuMusic();
        }

        if(levelMusicPlaying && !musicAud.isPlaying)
        {
            PlayLevelMusic();
        }
    }

    public void PlayMenuMusic()
    {
        menuMusicPlaying = true;
        levelMusicPlaying = false;

        int rand;
        do
        {
            rand = Random.Range(0, menuMusic.Length);
        }
        while (rand == prevMenuMusic);

        musicAud.Stop();
        musicAud.PlayOneShot(menuMusic[rand]);

        prevMenuMusic = rand;
    }

    public void PlayLevelMusic()
    {
        menuMusicPlaying = false;
        levelMusicPlaying = true;
        musicAud.Stop();
        musicAud.PlayOneShot(levelMusic);
    }

    public void HitNote(string name)
    {
        if (ShopInventoryLogic.instance.currentlyEquippedIndex == 9)
        {
            switch (name)
            {
                case "Re":
                    soundAud.PlayOneShot(bbre);
                    break;
                case "Mi":
                    soundAud.PlayOneShot(bbmi);
                    break;
                case "Fa":
                    soundAud.PlayOneShot(bbfa);
                    break;
                case "Sol":
                    soundAud.PlayOneShot(bbsol);
                    break;
                case "La":
                    soundAud.PlayOneShot(bbla);
                    break;
                case "Si":
                    soundAud.PlayOneShot(bbsi);
                    break;
            }
        }
        else if (ShopInventoryLogic.instance.currentlySpecialEquippedIndex == 8)
        {
            switch (name)
            {
                case "Re":
                    soundAud.PlayOneShot(hpre);
                    break;
                case "Mi":
                    soundAud.PlayOneShot(hpmi);
                    break;
                case "Fa":
                    soundAud.PlayOneShot(hpfa);
                    break;
                case "Sol":
                    soundAud.PlayOneShot(hpsol);
                    break;
                case "La":
                    soundAud.PlayOneShot(hpla);
                    break;
                case "Si":
                    soundAud.PlayOneShot(hpsi);
                    break;
            }
        }
        else
        {
            switch (name)
            {
                case "Re":
                    soundAud.PlayOneShot(re);
                    break;
                case "Mi":
                    soundAud.PlayOneShot(mi);
                    break;
                case "Fa":
                    soundAud.PlayOneShot(fa);
                    break;
                case "Sol":
                    soundAud.PlayOneShot(sol);
                    break;
                case "La":
                    soundAud.PlayOneShot(la);
                    break;
                case "Si":
                    soundAud.PlayOneShot(si);
                    break;
            }
        }
    }

    public void PlayerDestroyedSound()
    {
        soundAud.PlayOneShot(playerDestroyed);
    }
    public void StopPlayerDestroyedSound()
    {
        soundAud.Stop();
    }

    public void BoosterActivatedSound()
    {
        soundAud.PlayOneShot(boosterActivatedSound);
    }

    public void BoosterStopSound()
    {
        soundAud.Stop();
    }

    public void TapValueSound()
    {
        soundAud.PlayOneShot(tapCoin);
    }

    public void TapValueCeshSound()
    {
        soundAud.PlayOneShot(tapCesh);
    }

    public void TapGameBoosterSound()
    {
        soundAud.PlayOneShot(tapGameBooster);
    }
}
