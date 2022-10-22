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
            Debug.LogError("Next Scene Name not set. Using SceneIndex + 1.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }
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
