using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    public float fadeMusicTime = 3f;
    public float maxVolume = 1f;
    public float minVolume = 0.1f;

    void Awake()
    {
        instance = this;
    }
    
    public void PlaySong(AudioSource pianoTunes)
    {
        pianoTunes.volume = 0f;
        pianoTunes.Play();
        StartCoroutine(FadeMusic(pianoTunes, 0f, maxVolume));
        StartCoroutine(FadeMusic(GetComponent<AudioSource>(), maxVolume, minVolume));
        StartCoroutine(EndSong(pianoTunes));
    }

    IEnumerator EndSong(AudioSource pianoTunes)
    {
        yield return new WaitForSeconds(fadeMusicTime + pianoTunes.clip.length);
        StartCoroutine(FadeMusic(GetComponent<AudioSource>(), minVolume, maxVolume));
        StartCoroutine(FadeMusic(pianoTunes, maxVolume, 0f));
    }

    IEnumerator FadeMusic(AudioSource pianoTunes, float beforeVolume, float afterVolume)
    {
        float timeBegun = Time.time;
        while (true)
        {
            float timePassed = Time.time - timeBegun;
            if (timePassed > fadeMusicTime)
            {
                pianoTunes.volume = afterVolume;
                yield break;
            }
            pianoTunes.volume = Mathf.Lerp(beforeVolume, afterVolume, timePassed);
            yield return null;
        }
    }
}
