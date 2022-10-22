using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public bool isEnableOnce = true;

    private void OnTriggerEnter(Collider collider)
    {
        GameObject playerObject = collider.gameObject;
        if(playerObject.tag == "Player")
        {
            MusicPlayer.instance.PlaySong(GetComponent<AudioSource>());
            if (isEnableOnce)
            {
                enabled = false;
            }
        }
    }
}
