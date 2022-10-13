using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button_Load : MonoBehaviour
{
    [Tooltip("the name of the scene to load")]
    public string sceneToLoad;
    public GameObject loadingScreen;

    private Button button;
    private Slider slider;


    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate { LoadScene(sceneToLoad); });
        loadingScreen.transform.GetChild(0).GetComponent<Slider>();
    }

    void LoadScene(string tp_sceneToLoad)
    {
        if(sceneToLoad != "")
        {
            Debug.Log("Load scene");
            //SceneManager.LoadScene(tp_sceneToLoad);
            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsynchronously(tp_sceneToLoad));
        }
        else
        {
            Debug.LogError(gameObject.name + " Script ''Button_Load''  :  sceneToLoad NOT found. The name of the scene to load");
            Debug.LogError(gameObject.name + " Script ''Button_Load''  :  Scene must exist in Build Setting");
        }
    }

    IEnumerator LoadAsynchronously(string tp_sceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(tp_sceneToLoad);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading Progress = " + progress);

            yield return null;
        }
        
    }
}
