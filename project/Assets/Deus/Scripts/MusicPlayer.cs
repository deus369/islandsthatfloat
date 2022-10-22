using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    public float fadeMusicTime = 3f;
    public float maxVolume = 1f;
    public float minVolume = 0.1f;
    private Coroutine playSongRoutineA;
    private Coroutine playSongRoutineB;
    private Coroutine playSongRoutineC;

    void Awake()
    {
        instance = this;
    }
    
    public void PlaySong(AudioSource pianoTunes)
    {
        pianoTunes.volume = 0f;
        pianoTunes.Play();
        if (playSongRoutineA != null)
        {
            StopCoroutine(playSongRoutineA);
            StopCoroutine(playSongRoutineB);
        }
        if (playSongRoutineC != null)
        {
            StopCoroutine(playSongRoutineC);
        }
        playSongRoutineA = StartCoroutine(FadeMusic(pianoTunes, 0f, maxVolume));
        playSongRoutineB = StartCoroutine(FadeMusic(GetComponent<AudioSource>(), maxVolume, minVolume));
        playSongRoutineC = StartCoroutine(EndSong(pianoTunes));
    }

    IEnumerator EndSong(AudioSource pianoTunes)
    {
        yield return new WaitForSeconds(fadeMusicTime + pianoTunes.clip.length);
        StartCoroutine(FadeMusic(GetComponent<AudioSource>(), minVolume, maxVolume));
        StartCoroutine(FadeMusic(pianoTunes, maxVolume, 0f));
        playSongRoutineA = null;
        playSongRoutineB = null;
        playSongRoutineC = null;
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
        playSongRoutineA = null;
        playSongRoutineB = null;
    }
}
