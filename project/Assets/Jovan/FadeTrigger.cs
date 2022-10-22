using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeTrigger : MonoBehaviour
{
    // public int index;
    public float sec;
    public FadeManager Effect;
    public bool fadeOut;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Effect.Fade(true,1.80f);
            StartCoroutine(wait());
        } 
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(sec);
        fadeOut = true;
        // SceneManager.LoadScene(index);
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("Loading Gameplay Scene: " + nextSceneIndex);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
