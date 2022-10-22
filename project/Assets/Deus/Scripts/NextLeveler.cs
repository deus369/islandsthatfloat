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
            if (nextSceneName == "")
            {
                Debug.LogError("Next Scene Name not set.");
                return;
            }
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
