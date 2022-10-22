using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLeveler : MonoBehaviour
{
    public string nextSceneName;

    private void OnTriggerEnter(Collider collider)
    {
        GameObject playerObject = collider.gameObject;
        if(playerObject.tag == "Player")
        {
            OnTriggered();
        }
    }

    void OnTriggered()
    {
        if (nextSceneName == "")
        {
            var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log("NextLeveler.OnTriggered: Loading Next Scene: " + nextSceneIndex + " / " + SceneManager.sceneCountInBuildSettings);
                nextSceneIndex = 0;
            }
            Debug.Log("NextLeveler.OnTriggered: Loading Next Scene: " + nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
            return;
        }
        Debug.Log("Loading Next Scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    //! For Testing through game progression
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            OnTriggered();
        }
    }
}
