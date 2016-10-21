using UnityEngine;
using System.Collections;

public enum Sounds
{
    PrimeGun,
    PlaceSticker,
    CountDownBeep
}

public enum SoundSource
{
    Player,
    TagGun,
    GuiSource
}

public class SoundController : MonoBehaviour
{
    public AudioClip[] SoundEffects;
    public AudioSource PlayerAudioSource;
    public AudioSource TagGunAudioSource;
    public AudioSource GuiAudioSource;

    public void PlaySoundAtSourceOnce(SoundSource source, Sounds clip)
    {
        AudioSource playSource = new AudioSource();

        if (source == SoundSource.Player)
        {
            playSource = PlayerAudioSource;
        }
        else if (source == SoundSource.TagGun)
        {
            playSource = TagGunAudioSource;
        }
        else if (source == SoundSource.GuiSource)
        {
            playSource = GuiAudioSource;
        }

        playSource.PlayOneShot(SoundEffects[(int)clip]);
    }

}
